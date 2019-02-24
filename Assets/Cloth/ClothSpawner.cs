using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ClothSpawner : MonoBehaviour {

	// Simulation settings.
	public int resolution = 25;  // Not changeable at runtime.
	public float size = 10f;  // Not changeable at runtime.
	public int loops = 500;
	public float cor = 0.1f;
	public float mass = 1.0f;

	// parallel spring parameters
	public float pScale = 1.0f;
	public float pKs = -10000.0f;
	public float pKd = -1000.0f;

	// Diagonal spring parameters.
	public float dScale = 1.0f;
	public float dKs = -10000.0f;
	public float dKd = -1000.0f;

	// Bending spring parameters.
	public float bScale = 1.0f;
	public float bKs = -10000.0f;
	public float bKd = -1000.0f;

	// Wind / drag parameters.
	public float windScale = 1.0f;
	public float dragCoefficient = 50.0f;
	public Vector3 windVelocity = Vector3.zero;

	// Restrained vertex ids.
	public int[] restrained;

	// Mesh and mesh arrays.
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;

	// Compute shader and kernels.
	public ComputeShader clothCompute;
	private int springKernel;
	private int dragKernel;
	private int integrateKernel;

	// Compute buffers.
	private ComputeBuffer positionBuffer;
	private ComputeBuffer velocityBuffer;
	private ComputeBuffer forceBuffer;
	private ComputeBuffer restrainedBuffer;
	private ComputeBuffer triangleBuffer;

	// Other.
	private int count;  // total number of nodes.
	private int dim;  // number of nodes per row in cloth.
	private float segmentLength;  // length of one square of cloth.
	private Vector3[] zeros;
	private bool successfullyInitialized = false;

	// Collision.
	public GPUCollision.SphereCollider[] sphereColliders;
	private ComputeBuffer sphereBuffer;
	private int sphereCount;

	public GPUCollision.BoxCollider[] boxColliders;
	private ComputeBuffer boxBuffer;
	private int boxCount;


	void OnValidate() {
		// Update the resolution-related parameters.
		dim = resolution + 1;
		count = dim*dim;

		segmentLength = size / (float)resolution;
		
		// Recalculate the vertices array to be drawn as gizmos.
		vertices = new Vector3[count];
		for (int y = 0; y < dim; y++) {
			for (int x = 0; x < dim; x++) {
				vertices[y*dim+x] = new Vector3((float)x*segmentLength, this.transform.position.y, (float)y*segmentLength);
			}
		}
	}



	void Awake() {
		clothCompute = (ComputeShader)Instantiate(Resources.Load("ClothCompute"));

		// Create and set the mesh.
		mesh = CreateMesh("ClothMesh");
		GetComponent<MeshFilter>().mesh = mesh;

		// Create and set the position buffer.
		positionBuffer = new ComputeBuffer(count, 12);
		positionBuffer.SetData(vertices);

		// Create the zeros array.
		zeros = new Vector3[count];
		for (int i = 0; i < count; i++) {
			zeros[i] = Vector3.zero;
		}

		// Create and set the velocity buffer.
		velocityBuffer = new ComputeBuffer(count, 12);
		velocityBuffer.SetData(zeros);

		// Create and set the force buffer.
		forceBuffer = new ComputeBuffer(count, 12);
		forceBuffer.SetData(zeros);

		// Create an array representing which vertices to hold fixed.
		int[] restrainedArray = new int[count];
		for (int i = 0; i < count; i++) {
			restrainedArray[i] = (System.Array.Exists(restrained, element => element == i)) ? 1 : 0;
		}

		// Create and set the restrained buffer.
		restrainedBuffer = new ComputeBuffer(count, 4);
		restrainedBuffer.SetData(restrainedArray);

		// Create an array to hold the triangles as integer vectors.
		Vector3Int[] triangleArray = new Vector3Int[triangles.Length/3];
		for (int i = 0; i < triangles.Length; i += 3) {
			triangleArray[i/3] = new Vector3Int(triangles[i], triangles[i+1], triangles[i+2]);
		}

		// Create and set the triangle buffer.
		triangleBuffer = new ComputeBuffer(triangleArray.Length, 12);
		triangleBuffer.SetData(triangleArray);

		// Get the kernels from the compute shader.
		springKernel = clothCompute.FindKernel("Spring");
		dragKernel = clothCompute.FindKernel("Drag");
		integrateKernel = clothCompute.FindKernel("Integrate");

		// Upload the buffers to the gpu, and make them available to each kernel.
		clothCompute.SetBuffer(springKernel, "positionBuffer", positionBuffer);
		clothCompute.SetBuffer(springKernel, "velocityBuffer", velocityBuffer);
		clothCompute.SetBuffer(springKernel, "forceBuffer", forceBuffer);

		clothCompute.SetBuffer(dragKernel, "positionBuffer", positionBuffer);
		clothCompute.SetBuffer(dragKernel, "velocityBuffer", velocityBuffer);
		clothCompute.SetBuffer(dragKernel, "forceBuffer", forceBuffer);
		clothCompute.SetBuffer(dragKernel, "triangleBuffer", triangleBuffer);  // only need triangles for drag calculations.

		clothCompute.SetBuffer(integrateKernel, "positionBuffer", positionBuffer);
		clothCompute.SetBuffer(integrateKernel, "velocityBuffer", velocityBuffer);
		clothCompute.SetBuffer(integrateKernel, "forceBuffer", forceBuffer);
		clothCompute.SetBuffer(integrateKernel, "restrainedBuffer", restrainedBuffer);  // only need fixed vertices during integration phase.

		// Upload the constant parameters to the gpu.
		clothCompute.SetInt("count", count);
		clothCompute.SetInt("dim", dim);
		clothCompute.SetInt("triangleCount", triangleArray.Length);

		clothCompute.SetFloat("pRl", segmentLength);
		clothCompute.SetFloat("dRl", segmentLength * Mathf.Sqrt(2.0f));
		clothCompute.SetFloat("bRl", segmentLength * Mathf.Sqrt(2.0f) * 2.0f);

		// Create and set the collision buffers.
		if (sphereColliders != null && sphereColliders.Length > 0) {
			sphereBuffer = new ComputeBuffer(sphereColliders.Length, GPUCollision.SphereColliderSize());
			sphereBuffer.SetData(sphereColliders);
			clothCompute.SetBuffer(integrateKernel, "sphereBuffer", sphereBuffer);
		}
		sphereCount = sphereColliders.Length;
		clothCompute.SetInt("sphereCount", sphereCount);

		if (boxColliders != null && boxColliders.Length > 0) {
			boxBuffer = new ComputeBuffer(boxColliders.Length, GPUCollision.BoxColliderSize());
			boxBuffer.SetData(boxColliders);
			clothCompute.SetBuffer(integrateKernel, "boxBuffer", boxBuffer);
		}
		boxCount = boxColliders.Length;
		clothCompute.SetInt("boxCount", boxCount);

		// Initialization was successful.
		successfullyInitialized = true;
	}



	void FixedUpdate() {
		if (successfullyInitialized) {
			// Update the dynamic simulation variables.
			clothCompute.SetFloat("mass", mass);
			clothCompute.SetFloat("cor", cor);
			clothCompute.SetFloat("dt", Time.deltaTime/(float)loops);

			clothCompute.SetFloat("windScale", windScale);
			clothCompute.SetFloat("dragCoefficient", dragCoefficient);
			clothCompute.SetVector("windVelocity", windVelocity);

			clothCompute.SetFloat("pScale", pScale);
			clothCompute.SetFloat("pKs", pKs);
			clothCompute.SetFloat("pKd", pKd);

			clothCompute.SetFloat("dScale", dScale);
			clothCompute.SetFloat("dKs", dKs);
			clothCompute.SetFloat("dKd", dKd);
			
			clothCompute.SetFloat("bScale", bScale);
			clothCompute.SetFloat("bKs", bKs);
			clothCompute.SetFloat("bKd", bKd);

			// Update the collision buffers.
			/*
			if (sphereColliders.Length != sphereCount) {
				if (sphereBuffer != null) {
					sphereBuffer.Release();
				}

				if (sphereColliders.Length > 0) {
					sphereBuffer = new ComputeBuffer(sphereColliders.Length, GPUCollision.SphereColliderSize());
				}
			}
			*/
			sphereBuffer.SetData(sphereColliders);
			// sphereCount = sphereColliders.Length;
			// clothCompute.SetInt("sphereCount", sphereCount);
				


			if (boxColliders.Length != boxCount) {
				if (boxBuffer != null) {
					boxBuffer.Release();
				}
				
				if (boxColliders.Length > 0) {
					boxBuffer = new ComputeBuffer(boxColliders.Length, GPUCollision.SphereColliderSize());
					boxBuffer.SetData(boxColliders);
				}
				boxCount = boxColliders.Length;
			}

			// Advance the simulation n times.
			for (int i = 0; i < loops; i++) {
				// Clear the force buffer.
				forceBuffer.SetData(zeros);

				// Accumulate the spring and drag forces.
				clothCompute.Dispatch(springKernel, count/256+1, 1, 1);
				clothCompute.Dispatch(dragKernel, (triangles.Length/3)/256+1, 1, 1);

				// Update the simulation.
				clothCompute.Dispatch(integrateKernel, count/256+1, 1, 1);
			}

			// Get the recalculated positions back and set them as the mesh vertices.
			positionBuffer.GetData(vertices);
			mesh.vertices = vertices;

			// Recalculate the mesh normals.
			mesh.RecalculateNormals();
		}
	}



	void OnDestroy() {
		if (positionBuffer != null) {
			positionBuffer.Release();
		}

		if (velocityBuffer != null) {
			velocityBuffer.Release();
		}

		if (forceBuffer != null) {
			forceBuffer.Release();
		}

		if (restrainedBuffer != null) {
			restrainedBuffer.Release();
		}

		if (triangleBuffer != null) {
			triangleBuffer.Release();
		}

		if (sphereBuffer != null) {
			sphereBuffer.Release();
		}

		if (boxBuffer != null) {
			boxBuffer.Release();
		}
	}



	void OnDrawGizmos() {
		if (vertices != null && !successfullyInitialized) {
			for (int i = 0; i < vertices.Length; i++) {
				if (restrained != null && System.Array.Exists(restrained, element => element == i)) {
					Gizmos.color = Color.red;
					Gizmos.DrawSphere(vertices[i], segmentLength*0.25f);
				}
				else {
					Gizmos.color = Color.white;
					Gizmos.DrawSphere(vertices[i], segmentLength*0.125f);
				}
			}
		}

		if (sphereColliders != null) {
			foreach (GPUCollision.SphereCollider s in sphereColliders) {
				Gizmos.DrawWireSphere(s.center, s.radius);
			}
		}

		if (boxColliders != null) {
			foreach(GPUCollision.BoxCollider b in boxColliders) {
				Gizmos.DrawWireCube(b.center, b.extents*2f);
			}
		}
	}




	private Mesh CreateMesh(string name) {
		vertices = new Vector3[count];
		for (int y = 0; y < dim; y++) {
			for (int x = 0; x < dim; x++) {
				vertices[y*dim+x] = new Vector3((float)x*segmentLength, this.transform.position.y, (float)y*segmentLength);
			}
		}

		triangles = new int[resolution*resolution * 6];
		for (int ti = 0, vi = 0, y = 0; y < resolution; y++, vi++) {
			for (int x = 0; x < resolution; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + resolution + 1;
				triangles[ti + 5] = vi + resolution + 2;
			}
		}

		Vector2[] uvs = new Vector2[vertices.Length];
		for (int y = 0; y < dim; y++) {
			for (int x = 0; x < dim; x++) {
				float u = (float)x/(float)resolution;
				float v = (float)y/(float)resolution;
				uvs[y*dim+x] = new Vector2(u, v);
			}
		}

		Mesh mesh = new Mesh();
		mesh.name = name;
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();

		return mesh;
	}

	public bool WasSuccessfullyInitialized() {
		return successfullyInitialized;
	}
}
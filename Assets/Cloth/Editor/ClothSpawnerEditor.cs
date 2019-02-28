using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ClothSpawner))]
public class ClothSpawnerEditor : Editor {
	private bool showSimulationProperties = false;
	private bool showParallelSpringProperties = false;
	private bool showDiagonalSpringProperties = false;
	private bool showBendingSpringProperties = false;
	private bool showWindProperties = false;

	private SerializedProperty restrained;
	private SerializedProperty sphereColliders;

	void OnEnable() {
		restrained = serializedObject.FindProperty("restrained");
		sphereColliders = serializedObject.FindProperty("sphereColliders");
	}

	public override void OnInspectorGUI() {
		ClothSpawner clothSpawner = (ClothSpawner)target;

		EditorGUILayout.Space();
		showSimulationProperties = EditorGUILayout.Foldout(showSimulationProperties, "Simulation Properties", true);
		if (showSimulationProperties) {
			clothSpawner.positionOffset = EditorGUILayout.Vector3Field("Position Offset", clothSpawner.positionOffset);

			// Disable certain parameters that cannot be changed at runtime.
			if (clothSpawner.WasSuccessfullyInitialized()) {
				GUI.enabled = false;
			}

			clothSpawner.resolution = (int)Mathf.Max(EditorGUILayout.IntField("Cloth Resolution", clothSpawner.resolution), 1.0f);
			clothSpawner.size = Mathf.Max(EditorGUILayout.FloatField("Cloth Size", clothSpawner.size), 1.0f);

			GUI.enabled = true;

			clothSpawner.loops = (int)Mathf.Max(EditorGUILayout.IntField("Simulation Loops per Timestep", clothSpawner.loops), 1.0f);
			clothSpawner.cor = EditorGUILayout.Slider("Coefficient of Restitution", clothSpawner.cor, 0f, 1f);
			clothSpawner.mass = Mathf.Max(EditorGUILayout.FloatField("Point Mass", clothSpawner.mass), 0.001f);
		}

		EditorGUILayout.Space();
		showParallelSpringProperties = EditorGUILayout.Foldout(showParallelSpringProperties, "Parallel Spring Properties", true);
		if (showParallelSpringProperties) {
			clothSpawner.pScale = EditorGUILayout.Slider("Contribution", clothSpawner.pScale, 0f, 1f);
			clothSpawner.pKs = Mathf.Min(EditorGUILayout.FloatField("Spring Constant (ks)", clothSpawner.pKs), -1.0f);
			clothSpawner.pKd = Mathf.Min(EditorGUILayout.FloatField("Damping Constant (kd)", clothSpawner.pKd), 0.0f);
		}

		EditorGUILayout.Space();
		showDiagonalSpringProperties = EditorGUILayout.Foldout(showDiagonalSpringProperties, "Diagonal Spring Properties", true);
		if (showDiagonalSpringProperties) {
			clothSpawner.dScale = EditorGUILayout.Slider("Contribution", clothSpawner.dScale, 0f, 1f);
			clothSpawner.dKs = Mathf.Min(EditorGUILayout.FloatField("Spring Constant (ks)", clothSpawner.dKs), -1.0f);
			clothSpawner.dKd = Mathf.Min(EditorGUILayout.FloatField("Damping Constant (kd)", clothSpawner.dKd), 0.0f);
		}

		EditorGUILayout.Space();
		showBendingSpringProperties = EditorGUILayout.Foldout(showBendingSpringProperties, "Bending Spring Properties", true);
		if (showBendingSpringProperties) {
			clothSpawner.bScale = EditorGUILayout.Slider("Contribution", clothSpawner.bScale, 0f, 1f);
			clothSpawner.bKs = Mathf.Min(EditorGUILayout.FloatField("Spring Constant (ks)", clothSpawner.bKs), -1.0f);
			clothSpawner.bKd = Mathf.Min(EditorGUILayout.FloatField("Damping Constant (kd)", clothSpawner.bKd), 0.0f);
		}

		EditorGUILayout.Space();
		showWindProperties = EditorGUILayout.Foldout(showWindProperties, "Wind Properties", true);
		if (showWindProperties) {
			clothSpawner.windScale = EditorGUILayout.Slider("Contribution", clothSpawner.windScale, 0f, 1f);
			clothSpawner.dragCoefficient = Mathf.Max(EditorGUILayout.FloatField("Drag Coefficient (cd)", clothSpawner.dragCoefficient), 1.0f);
			clothSpawner.windVelocity = EditorGUILayout.Vector3Field("Wind Velocity", clothSpawner.windVelocity);
		}

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(restrained, true);
		serializedObject.ApplyModifiedProperties();

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(sphereColliders, true);
		serializedObject.ApplyModifiedProperties();

		clothSpawner.Recalculate();
	}
	
}

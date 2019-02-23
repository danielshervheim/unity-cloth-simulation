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

	void OnEnable() {
		restrained = serializedObject.FindProperty("restrained");
	}

	public override void OnInspectorGUI() {
		ClothSpawner clothSpawner = (ClothSpawner)target;

		EditorGUILayout.Space();
		showSimulationProperties = EditorGUILayout.Foldout(showSimulationProperties, "Simulation Properties", true);
		if (showSimulationProperties) {
			// Disable certain parameters that cannot be changed at runtime.
			if (clothSpawner.WasSuccessfullyInitialized()) {
				GUI.enabled = false;
			}

			clothSpawner.resolution = EditorGUILayout.IntField("Cloth Resolution", clothSpawner.resolution);
			clothSpawner.size = EditorGUILayout.FloatField("Cloth Size", clothSpawner.size);

			GUI.enabled = true;

			clothSpawner.loops = EditorGUILayout.IntField("Simulation Loops per Timestep", clothSpawner.loops);
			clothSpawner.cor = EditorGUILayout.Slider("Coefficient of Restitution", clothSpawner.cor, 0f, 1f);
			clothSpawner.mass = EditorGUILayout.FloatField("Point Mass", clothSpawner.mass);
		}

		EditorGUILayout.Space();
		showParallelSpringProperties = EditorGUILayout.Foldout(showParallelSpringProperties, "Parallel Spring Properties", true);
		if (showParallelSpringProperties) {
			clothSpawner.pScale = EditorGUILayout.Slider("Contribution", clothSpawner.pScale, 0f, 1f);
			clothSpawner.pKs = EditorGUILayout.FloatField("Spring Constant (ks)", clothSpawner.pKs);
			clothSpawner.pKd = EditorGUILayout.FloatField("Damping Constant (kd)", clothSpawner.pKd);
		}

		EditorGUILayout.Space();
		showDiagonalSpringProperties = EditorGUILayout.Foldout(showDiagonalSpringProperties, "Diagonal Spring Properties", true);
		if (showDiagonalSpringProperties) {
			clothSpawner.dScale = EditorGUILayout.Slider("Contribution", clothSpawner.dScale, 0f, 1f);
			clothSpawner.dKs = EditorGUILayout.FloatField("Spring Constant (ks)", clothSpawner.dKs);
			clothSpawner.dKd = EditorGUILayout.FloatField("Damping Constant (kd)", clothSpawner.dKd);
		}

		EditorGUILayout.Space();
		showBendingSpringProperties = EditorGUILayout.Foldout(showBendingSpringProperties, "Bending Spring Properties", true);
		if (showBendingSpringProperties) {
			clothSpawner.bScale = EditorGUILayout.Slider("Contribution", clothSpawner.bScale, 0f, 1f);
			clothSpawner.bKs = EditorGUILayout.FloatField("Spring Constant (ks)", clothSpawner.bKs);
			clothSpawner.bKd = EditorGUILayout.FloatField("Damping Constant (kd)", clothSpawner.bKd);
		}

		EditorGUILayout.Space();
		showWindProperties = EditorGUILayout.Foldout(showWindProperties, "Wind Properties", true);
		if (showWindProperties) {
			clothSpawner.windScale = EditorGUILayout.Slider("Contribution", clothSpawner.windScale, 0f, 1f);
			clothSpawner.dragCoefficient = EditorGUILayout.FloatField("Drag Coefficient (cd)", clothSpawner.dragCoefficient);
			clothSpawner.windVelocity = EditorGUILayout.Vector3Field("Wind Velocity", clothSpawner.windVelocity);
		}

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(restrained, true);
		serializedObject.ApplyModifiedProperties();
	}
	
}

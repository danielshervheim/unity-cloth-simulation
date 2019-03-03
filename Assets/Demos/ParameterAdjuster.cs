using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterAdjuster : MonoBehaviour {
	public int loops = 250;

	public Vector3 windVelocity = Vector3.zero;

	public ClothSpawner[] cloths;

	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 75, 25), "Decrease")) {
			loops -= 5;
		}
		if (GUI.Button(new Rect(10, 35, 75, 25), "Increase")) {
			loops += 5;
		}

		GUI.Label(new Rect(10, 60, 75, 25), "" + loops); 

		loops = (int)Mathf.Max(loops, 1f);
	}

	void FixedUpdate() {
		for (int i = 0; i < cloths.Length; i++) {
			cloths[i].loops = loops;
			cloths[i].windVelocity = windVelocity;
		}
	}
}

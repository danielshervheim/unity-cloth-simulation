using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterAdjuster : MonoBehaviour {
	public int loops = 250;

	public float k = -10000f;

	public ClothSpawner[] cloths;

	void OnGUI() {
		// Loops

		if (GUI.Button(new Rect(10, 10, 150, 20), "Decrease Loops")) {
			loops -= 10;
		}

		if (GUI.Button(new Rect(10, 40, 150, 20), "Increase Loops")) {
			loops += 10;
		}

		GUI.Label(new Rect(10, 70, 100, 20), "" + loops);

		loops = (int)Mathf.Max(loops, 1f);

		// Constant

		if (GUI.Button(new Rect(10, 100, 150, 20), "Decrease Constant")) {
			k *= 2f;
		}

		if (GUI.Button(new Rect(10, 130, 150, 20), "Increase Constant")) {
			k /= 2f;
		}

		GUI.Label(new Rect(10, 160, 100, 20), "" + k);

		k = Mathf.Min(k, -1f);
	}

	void FixedUpdate() {
		for (int i = 0; i < cloths.Length; i++) {
			cloths[i].loops = loops;
			cloths[i].pKs = k;
			cloths[i].dKs = k;
			cloths[i].bKs = k;
		}
	}
}

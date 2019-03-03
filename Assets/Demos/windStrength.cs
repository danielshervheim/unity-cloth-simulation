using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windStrength : MonoBehaviour {

	private float t;
	public ClothSpawner cs;
	
	// Update is called once per frame
	void FixedUpdate () {
		t += Time.deltaTime;
		cs.windScale = 0.5f*(Mathf.Sin(t) + 1f);
	}
}

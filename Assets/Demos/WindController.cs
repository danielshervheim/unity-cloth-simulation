using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour {

	public ClothSpawner cs;

	private float x = 0;
	private float y = 0;
	private float z = 0;

	void OnGUI () {
		x = GUI.VerticalSlider(new Rect(10, 10, 25, 100), x, 10.0f, -10.0f);
    	y = GUI.VerticalSlider(new Rect(10, 120, 25, 100), y, 10.0f, -10.0f);
    	z = GUI.VerticalSlider(new Rect(10, 230, 25, 100), z, 10.0f, -10.0f);

    	cs.windVelocity = new Vector3(x, y, z);
	}
}

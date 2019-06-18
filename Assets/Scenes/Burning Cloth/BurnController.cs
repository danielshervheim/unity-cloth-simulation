using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnController : MonoBehaviour {

	public Material burningCloth;

	private bool burn = false;
	private float t = 0f;

	public float burnDuration = 0.25f;

	public Transform lights;
	private Light[] lightComps;

	public GameObject flameParticles;
	private GameObject flames;

	private Vector3 ogPos;

	public AnimationCurve particleOffset;

	void Start() {
		ogPos = lights.position;
		lightComps = lights.GetComponentsInChildren<Light>();
	}

	void OnGUI () {
		if (GUI.Button(new Rect(10, 10, 75, 25), burn ? "Unburn" : "Burn")) {
			burn = !burn;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (burn) {
			t += Time.deltaTime * burnDuration;

			//if (flames == null) {
			//	flames = Instantiate(flameParticles, lights.position, Quaternion.identity, null);
			//}
		}
		else {
			t -= Time.deltaTime * burnDuration;
		}


		t = Mathf.Clamp(t, 0f, 1f);
		burningCloth.SetFloat("_BurnAmount", t);

		foreach (Light l in lightComps) {
			l.intensity = Mathf.Lerp(0f, 1.75f, Mathf.Clamp(t, 0f, 0.1f)/0.1f);
		}

		lights.position = Vector3.Lerp(ogPos, ogPos + Vector3.up * 10f, t);

		//flames.transform.position = lights.position + Vector3.up*particleOffset.Evaluate(t);
	}
}

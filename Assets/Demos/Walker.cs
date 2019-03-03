using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour {

	public float walkSpeed = 1.25f;
	public float rotateSpeed = 1f;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetFloat("Blend", Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f));
		transform.position += transform.forward * Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f) * Time.deltaTime * walkSpeed;
		transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * Input.GetAxis("Horizontal"));
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float maxSpeed;
	public float acceleration;
	private Vector3 velocity;
	private float friction = 0.9f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

		if (velocity.magnitude < maxSpeed) { 
			velocity += input * acceleration * Time.deltaTime;
		}
		if (input == new Vector3(0,0,0)) {
			velocity *= friction;
		}
		transform.Translate (velocity * Time.deltaTime);
	}
}

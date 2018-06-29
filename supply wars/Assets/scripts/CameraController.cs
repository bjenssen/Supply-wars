using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float maxSpeed;
	public float acceleration;
	public float scrollSpeed;
	public float maxHeight;
	public float minHeight;

	private Vector3 velocity;
	private float friction = 0.9f;
	private Vector3 input = new Vector3 (0, 0, 0);


	
	// Update is called once per frame
	void Update () {
		input = new Vector3 (Input.GetAxisRaw ("Horizontal"), -scrollSpeed * Input.GetAxis ("Mouse ScrollWheel"), Input.GetAxisRaw ("Vertical"));
		if (velocity.magnitude < maxSpeed * transform.position.y) { 
			velocity += input * acceleration * transform.position.y * Time.deltaTime;
		}
		velocity *= friction;
		if ((minHeight >= transform.position.y || Input.GetAxis ("Mouse ScrollWheel") < 0) && (maxHeight <= transform.position.y || Input.GetAxis ("Mouse ScrollWheel") > 0)) {
			velocity = new Vector3 (velocity.x, 0, velocity.z);
		}
		if (transform.position.y < minHeight)
			transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
		if (transform.position.y > maxHeight)
			transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
		transform.Translate (velocity * Time.deltaTime,Space.World);
	}
}

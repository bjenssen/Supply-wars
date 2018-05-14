using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {
	
	public GameObject m_Plane;
	public float speed;
	private Vector3 hitPoint;
	private Vector3 point1;
	private Vector3 point2;
	private float A;
	private float B;
	private float C;
	private float D;
	private bool moving = false;

	// Use this for initialization
	void Start () {
		hitPoint = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				point1 = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
				Debug.Log("1");
			}
		}
		if (Input.GetMouseButtonUp(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				point2 = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
				Debug.Log("2");
			}
			if (point1.x < point2.x) {
				A = point1.x;
				B = point2.x;
			} else {
				A = point2.x;
				B = point1.x;
			}
			if (point1.z < point2.z) {
				C = point1.z;
				D = point2.z;
			} else {
				C = point2.z;
				D = point1.z;
			}
			if (A <= transform.position.x && B >= transform.position.x && C <= transform.position.z && D >= transform.position.z) {
				GetComponent<SelectionScript> ().selected = true;
			}
		}
		if (GetComponent<SelectionScript>().selected) {
			if (Input.GetMouseButtonDown (1)) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit)) {
					hitPoint = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
					moving = true;
				}
			}
		}
		float step = speed * Time.deltaTime;
		if (moving == true) {
			transform.position = Vector3.MoveTowards (transform.position, hitPoint, step);
			if (transform.position == hitPoint) {
				moving = false;
			}
		}

	}
}

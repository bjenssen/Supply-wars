using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoxSelectionScript : MonoBehaviour {

	public LayerMask unitMask;

	private Vector3 hitPoint;
	private Vector3 point1;
	private Vector3 point2;
	private Vector3 Point;
	public string mouseX;
	public string mouseY;
	private float A;
	private float B;
	private float C;
	private float D;
	private int width;
	public float spacing;

	public event Action boxSelect;

	public static BoxSelectionScript instance;

	Collider[] hitColliders;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			DestroyImmediate (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;

			//new Vector2 mousePos =
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				hitPoint = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
			}
			int i = 0;
			int n = 0;
			int w = 0;
			int x = 0;
			int y = 0;
			while (i < hitColliders.Length) {
				if (hitColliders [i].tag == "Unit") {
					x = n % width;
					y = n / width;
					Point = new Vector3 (hitPoint.x + (x * spacing), 0.1f, hitPoint.z + (y * spacing));
					UnitScript u = hitColliders [i].GetComponent<UnitScript> ();
					u.hitPoint = Point;
					u.moving = true;
					u.attacking = false;
					i++;
					n++;
				} else {
					x = -2;
					y = w;
					Point = new Vector3 (hitPoint.x + (x * spacing), 0.1f, hitPoint.z + (y * spacing * 3));
					WagonScript u = hitColliders [i].GetComponent<WagonScript> ();
					u.hitPoint = Point;
					u.moving = true;
					i++;
					w++;
				}
			}
		}
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				point1 = new Vector3 (hit.point.x, 0, hit.point.z);
			}
		}
		if (Input.GetMouseButton (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				point2 = new Vector3 (hit.point.x, 0, hit.point.z);
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
			MyCollisions ();
		}

	}
	void MyCollisions()
	{
		boxSelect ();
		hitColliders = Physics.OverlapBox ((point1 + point2) / 2, new Vector3 ((B - A) / 2, 2, (D - C) / 2),Quaternion.identity,unitMask);
		Debug.DrawLine (new Vector3 (A, 1, C), new Vector3 (A, 1, D));
		Debug.DrawLine (new Vector3 (B, 1, C), new Vector3 (B, 1, D));
		Debug.DrawLine (new Vector3 (A, 1, C), new Vector3 (B, 1, C));
		Debug.DrawLine (new Vector3 (A, 1, D), new Vector3 (B, 1, D));

		Debug.DrawLine ((point1 + point2) / 2, (point1 + point2) / 2 + new Vector3 (B - A / 2, 2, D - C / 2).x * Vector3.right);

		width = (int)Math.Sqrt(hitColliders.Length);
		int i = 0;
		while (i < hitColliders.Length) {
			//Debug.Log ("hit : " + hitColliders [i].name + i);
			SelectionScript s = hitColliders [i].GetComponent<SelectionScript> ();
			if(s != null)
				s.selected = true;
			i++;
		}

	}
}


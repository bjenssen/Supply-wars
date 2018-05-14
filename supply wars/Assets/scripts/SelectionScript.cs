using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScript : MonoBehaviour {

	public int supply = 0;
	public bool selected = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) {
				if (hit.transform.name == this.gameObject.name) {
					selected = true;
				} else selected = false;
			} 
		}
		if (selected) {
			gameObject.GetComponent<Renderer>().material.color = Color.cyan;
		} else gameObject.GetComponent<Renderer>().material.color = Color.white;
	}
}

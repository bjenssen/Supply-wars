using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScript : MonoBehaviour {


	public bool selected = false;

	bool isSelecting;
	// Use this for initialization
	void Start () {
		BoxSelectionScript.instance.boxSelect += SelectHandler;
	}

	void SelectHandler(){
		selected = false;
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
	}
}

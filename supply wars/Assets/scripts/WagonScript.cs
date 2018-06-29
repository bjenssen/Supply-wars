using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WagonScript : MonoBehaviour {


	public NavMeshAgent agent;
	public Vector3 hitPoint;
	public Vector3 collectPoint;
	public int supply = 0;
	public bool collecting = false;
	public bool moving = false;
	public int maxSupply = 500;
	public bool taken = false;
	public GameObject supplyBar;
	public GameObject supplybar;
	public GameObject redBar;
	public GameObject redbar;

	void Start () {
		hitPoint = transform.position;
		supplybar = Instantiate(supplyBar, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
		redbar = Instantiate(redBar, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
		redbar.transform.localScale = new Vector3(0.5f, 0.015f, 0.015f);
	}

	// Update is called once per frame
	void Update () {
		if (GetComponent<SelectionScript> ().selected) {
			supplybar.active = true;
			supplybar.transform.position = new Vector3(transform.position.x+(supply-500)/2000.0f, supplybar.transform.position.y, transform.position.z);
			supplybar.transform.localScale = new Vector3(supply/1000.0f, 0.02f, 0.02f);
			redbar.active = true;
			redbar.transform.position = new Vector3(transform.position.x, redbar.transform.position.y, transform.position.z);
		} else {
			supplybar.active = false;
			redbar.active = false;
		}
		if (collecting && !moving && !taken)
			agent.SetDestination (collectPoint);
		else if (!collecting && Vector3.Distance(transform.position, hitPoint) >= 0.2 && !moving)
			moving = true;
		if (Vector3.Distance(transform.position, hitPoint) >= 0.2 && (moving || taken))
			agent.SetDestination (hitPoint);
		else
			moving = false;
	}
}

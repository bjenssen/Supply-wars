using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {


	public UnityEngine.AI.NavMeshAgent agent;
	public WagonScript closestWagon;
	public int health = 100;
	private GameControllerScript list;
	private float d;
	private Vector3 spawnpoint;
	private bool taking = false;
	public AudioClip scream;
	private bool screamed = false;
	public AudioClip death1;
	public AudioClip death2;

	void Start () {
		list = GameObject.Find ("GameController").GetComponent<GameControllerScript> ();
		spawnpoint = transform.position;
	}

	void Update () {
		for (int i = 0; i < list.wagons.Count; i++) {
			if (closestWagon == null) {
				closestWagon = list.wagons [i];
				d = Vector3.Distance (list.wagons [i].transform.position, transform.position);
			} else if (Vector3.Distance (list.wagons [i].transform.position, transform.position) <= d) {
				closestWagon = list.wagons [i];
				d = Vector3.Distance (list.wagons [i].transform.position, transform.position);
			}
		}
		if (d < 10.0f && d > 9.0f && !screamed && !list.scream){
			list.scream = true;
			screamed = true;
			list.fightingSource.PlayOneShot(scream, 3.0f);
			list.marchingSource.PlayOneShot(scream, 3.0f);
			list.timer2 = Random.Range(5.0f, 8.0f);
		}
		if (d <= 0.5f && !closestWagon.taken) {
			closestWagon.taken = true;
			taking = true;
		}
		if (taking && Vector3.Distance (closestWagon.transform.position, transform.position) <= 0.5f) {
			agent.SetDestination (spawnpoint);
			agent.speed = closestWagon.agent.speed;
			closestWagon.hitPoint = transform.position;
		} else {
			agent.SetDestination (closestWagon.transform.position);
			if (closestWagon.taken && d <= 0.5f)
				agent.speed = closestWagon.agent.speed;
			else 
				agent.speed = 1;
		}
		if (health <= 0) {
			if (closestWagon.taken) {
				closestWagon.taken = false;
			}
			if (!list.death) {
				list.death = true;
				list.fightingSource.PlayOneShot(death1, 1.0f);
				list.marchingSource.PlayOneShot(death2, 1.0f);
				list.timer3 = Random.Range(0.5f, 1.0f);
			}
			Destroy (gameObject);
		}
	}
}

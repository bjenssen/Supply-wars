using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class millScript : MonoBehaviour {

	public int supply = 1000;
	public GameObject supplyBar;
	public GameObject supplybar;
	public GameObject redBar;
	public GameObject redbar;

	private float timer = 10.0f;
	private int maxSupply = 1000;
	private int gains = 15;
	private float range = 10.0f;
	private GameControllerScript list;

	void Start () {
		list = FindObjectOfType<GameControllerScript>();
		supplybar = Instantiate(supplyBar, new Vector3(transform.position.x, transform.position.y + 4.0f, transform.position.z - 0.2f), transform.rotation);
		redbar = Instantiate(redBar, new Vector3(transform.position.x, transform.position.y + 4.0f, transform.position.z - 0.2f), transform.rotation);
		redbar.transform.localScale = new Vector3(1.0f, 0.04f, 0.04f);
	}

	void Update () {
		supplybar.transform.position = new Vector3(transform.position.x+(supply-1000)/2000.0f, supplybar.transform.position.y, transform.position.z - 0.2f);
		supplybar.transform.localScale = new Vector3(supply/1000.0f, 0.05f, 0.05f);
		redbar.transform.position = new Vector3(transform.position.x, redbar.transform.position.y, transform.position.z - 0.2f);

		for (int i = 0; i < list.wagons.Count; i++) {
			if (!list.wagons [i].moving && Vector3.Distance(list.wagons [i].transform.position, transform.position) <= range && list.wagons [i].supply <= 400 && supply >= 100) {
				list.wagons [i].collecting = true;
				list.wagons [i].collectPoint = new Vector3(transform.position.x, 0.1f, transform.position.z+1.5f);
				if (Vector3.Distance(list.wagons [i].transform.position, transform.position) <= 2.0f) {
					while (list.wagons [i].supply < list.wagons [i].maxSupply + gains && supply >= gains) {
						supply -= gains;
						list.wagons [i].supply += gains;
					}
					list.wagons [i].collecting = false;
				}
			}
		}
		timer -= Time.deltaTime;
		if (timer <= 0.0f) {
			timerEnded ();
			timer = 10.0f;
		}
	}

	void timerEnded() {
		if (supply <= maxSupply)
			supply += gains;
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {


	public List<WagonScript> wagons = new List<WagonScript>();
	public List<GameObject> units = new List<GameObject>();
	public int enemyNumber;
	public int waveMultiplier;
	public GameObject enemy;
	public GameObject target;
	public LayerMask enemyLayer;
	public LayerMask unitLayer;
	public Texture2D foreground;
	public Texture2D background;
	public Texture2D supplyMeter;
	
	private GameObject[] spawnerArray;

	//the activated spawner
	private int sNumber = 0;
	//the number of activated spawnpoints
	private int sAmount = 0;
	//the wavenumber
	private int wave = 0;
	private float timer = 0.0f;

	private float d;


	void Start () {
		GameObject[] Array = GameObject.FindGameObjectsWithTag ("Wagon");
		foreach (GameObject obj in Array) {
			wagons.Add(obj.GetComponent<WagonScript>());
		}
		Array = GameObject.FindGameObjectsWithTag ("Unit");
		foreach (GameObject obj in Array) {
			units.Add(obj);
		}
		spawnerArray = GameObject.FindGameObjectsWithTag ("Spawner");
	}

	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0.0f) {
			timerEnded ();
			timer = 30.0f;
		}
		for (int i = 0; i < units.Count; i++) {
			if (units[i] != null) {
				UnitScript uScript = units[i].GetComponent<UnitScript> ();
				if (units[i].GetComponent<SelectionScript> ().selected){
					uScript.greenbar.active = true;
					uScript.redbar.active = true;
					uScript.supplybar.active = true;
					
					uScript.greenbar.transform.position = new Vector3(units[i].transform.position.x+(uScript.health-100)/2000.0f, uScript.greenbar.transform.position.y, units[i].transform.position.z);
					uScript.greenbar.transform.localScale = new Vector3(uScript.health/1000.0f, 0.01f, 0.01f);
					uScript.redbar.transform.position = new Vector3(units[i].transform.position.x, uScript.redbar.transform.position.y, units[i].transform.position.z);
					uScript.supplybar.transform.position = new Vector3(units[i].transform.position.x+(uScript.supply-5)/100.0f, uScript.supplybar.transform.position.y, units[i].transform.position.z);
					uScript.supplybar.transform.localScale = new Vector3(uScript.supply/100.0f, 0.01f, 0.01f);
				} else {
					uScript.greenbar.active = false;
					uScript.redbar.active = false;
					uScript.supplybar.active = false;
				}
				Collider[] hitColliders = Physics.OverlapSphere(units[i].transform.position, 3.0f, enemyLayer);
				float maxdist = float.MaxValue;
				target = null;
				foreach(Collider c in hitColliders){
					float d = Vector3.Distance(units[i].transform.position,c.transform.position);
					if (d < maxdist){
						maxdist = d;
						target = c.gameObject;
					}
				}
				if(target != null && !uScript.moving)
				{
					uScript.hitPoint = target.transform.position;
					if (Vector3.Distance(units[i].transform.position, target.transform.position) <= 0.5f && uScript.attacking == false) {
						uScript.attacking = true;
						target.GetComponent<EnemyScript> ().health -= 20;
					}
				}
			}
		}
	}

	void timerEnded () {
		wave += 1;
		enemyNumber += waveMultiplier;
		if (wave == 10 || wave == 20) {
			sAmount += 1;
			enemyNumber /= 2;
		}
		for (int j = 0; j <= sAmount; j++) {
			sNumber = Random.Range (0, spawnerArray.Length);
			for (int k = 0; k <= enemyNumber; k++) {
				Instantiate (enemy, spawnerArray [sNumber].transform.position + new Vector3 (Random.Range (-5.0f, 5.0f), 0.0f, Random.Range (-5.0f, 5.0f)), spawnerArray [sNumber].transform.rotation);
			}
		}
	}
}

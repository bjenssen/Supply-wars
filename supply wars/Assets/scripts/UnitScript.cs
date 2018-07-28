using UnityEngine;
using UnityEngine.AI;

public class UnitScript : MonoBehaviour {


	public NavMeshAgent agent;
	public Vector3 hitPoint;
	public WagonScript closestWagon;
	public int supply = 5;
	public int health = 100;
	public bool moving = false;
	public bool attacking = false;
	public GameObject greenBar;
	public GameObject redBar;
	public GameObject supplyBar;
	public GameObject greenbar;
	public GameObject redbar;
	public GameObject supplybar;

	private float d;
	private float timer = 10.0f;
	private float attackTimer = 1.0f;
	private int maxSupply = 10;
	private float maxHealth = 100;
	private float range = 5.0f;
	public bool collecting = false;
	private GameControllerScript list;
	private BoxSelectionScript bss;
	public Animator m_Animator;

	void Start () {
		hitPoint = transform.position;
		m_Animator = GetComponentInChildren<Animator>();
		list = FindObjectOfType<GameControllerScript>();
		bss = FindObjectOfType<BoxSelectionScript>();
		greenbar = Instantiate(greenBar, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
		redbar = Instantiate(redBar, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
		supplybar = Instantiate(supplyBar, new Vector3(transform.position.x, transform.position.y + 0.45f, transform.position.z), transform.rotation);
		redbar.transform.localScale = new Vector3(0.1f, 0.009f, 0.009f);
	}

	void Update () {
		m_Animator.speed = 1;
		for (int i = 0; i < list.wagons.Count; i++) {
			if (closestWagon == null && (list.wagons [i].supply > 0 || i == list.wagons.Count-1)) {
				closestWagon = list.wagons [i];
				d = Vector3.Distance (list.wagons [i].transform.position, transform.position);
			} else if (Vector3.Distance (list.wagons [i].transform.position, transform.position) <= d && list.wagons [i].supply > 0) {
				closestWagon = list.wagons [i];
				d = Vector3.Distance (list.wagons [i].transform.position, transform.position);
			}
		}
		transform.eulerAngles += new Vector3 (-90, 0, 0);
		if (attacking) {
			m_Animator.SetBool ("isAttacking", true);
			m_Animator.SetBool ("isWalking", false);
			attackTimer -= Time.deltaTime;
			if (attackTimer <= 0.0f) {
				attacking = false;
				attackTimer = 1.0f;
			}
		} else if (collecting && !moving) {
			agent.SetDestination (closestWagon.transform.position);
			m_Animator.SetBool ("isWalking", true);
			m_Animator.SetBool ("isAttacking", false);
		} else if (Vector3.Distance(transform.position, hitPoint) >= 0.2f) {
			//transform.rotation = Quaternion.LookRotation(m_Rigidbody.velocity);
			m_Animator.SetBool ("isWalking", true);
			m_Animator.SetBool ("isAttacking", false);
			//m_Animator.speed = m_Rigidbody.velocity.magnitude / m_MoveSpeedMultiplier;
			agent.SetDestination (hitPoint);
		} else {
			m_Animator.SetBool ("isWalking", false);
			m_Animator.SetBool ("isAttacking", false);
			moving = false;
		}
		timer -= Time.deltaTime;
		if (timer <= 0.0f) {
			timerEnded ();
			timer = 10.0f;
		}
		if (supply <= 5 && Vector3.Distance(closestWagon.transform.position, transform.position) <= range) {
			if (!moving && !attacking && closestWagon.supply >= 1) {
				collecting = true;
			}
		}
		if (collecting && Vector3.Distance(closestWagon.transform.position, transform.position) <= 0.5f) {
			while (supply < maxSupply && closestWagon.supply >= 1) {
				supply += 1;
				closestWagon.supply -= 1;
			}
			collecting = false;
		}
		if (health <= 0) {
			Destroy (gameObject);
			Destroy (redbar);
			Destroy (greenbar);
			Destroy (supplybar);
		}
	}

	void timerEnded() {
		if (supply == 0) {
			health -= 20;
		} else
			supply -= 1;
		if (supply != 0 && health < maxHealth)
			health += 20;
	}
}

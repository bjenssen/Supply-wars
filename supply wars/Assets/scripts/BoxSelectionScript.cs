using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoxSelectionScript : MonoBehaviour {

	public LayerMask unitMask;

	private Vector3 hitPoint;
	private static Vector3 point1;
	private static Vector3 point2;
	private Vector3 Point;
	public GameObject flag;
	public string mouseX;
	public string mouseY;
	private static float A;
	private static float B;
	private static float C;
	private static float D;
	private int width;
	private Vector3 posThickness = new Vector3(10, 10, 0);
	private bool sndReady = false;
	Vector3 mousePosition1;
	Array[] GuiPosition;
	public float spacing;

	public event Action boxSelect;

	public static BoxSelectionScript instance;

	Collider[] hitColliders;

	public AudioSource source;
	public AudioClip ready;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			DestroyImmediate (gameObject);
		}
	}
	static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if( _whiteTexture == null )
            {
                _whiteTexture = new Texture2D( 1, 1 );
                _whiteTexture.SetPixel( 0, 0, Color.white );
                _whiteTexture.Apply();
            }
 
            return _whiteTexture;
        }
    }
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (1)) {
			mousePosition1 = Input.mousePosition;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				hitPoint = new Vector3 (hit.point.x, 0.4f, hit.point.z);
			}
			if (flag) {
				GameObject.Destroy(flag);
			}
			flag = Instantiate (flag, hitPoint, new Quaternion(0,0,0,0));
			int i = 0;
			int n = 0;
			int w = 0;
			int x = 0;
			int y = 0;
			while (i < hitColliders.Length) {
				// if (Physics.Raycast(ray, out hit)) {
				// 	if (hit.transform.tag == "Wagon" && hitColliders [i].tag == "Unit") {
				// 		UnitScript u = hitColliders [i].GetComponent<UnitScript> ();
				// 		u.collecting = true;
				// 		u.moving = false;
				// 		u.attacking = false;
				// 	} else if (hit.transform.tag == "Enemy" && hitColliders [i].tag == "Unit") {
				// 		UnitScript u = hitColliders [i].GetComponent<UnitScript> ();
				// 		u.collecting = false;
				// 		u.moving = false;
				// 		u.attacking = true;
				// 	}
				// } else 
				if (hitColliders [i].tag == "Unit") {
					x = n % width;
					y = n / width;
					Point = new Vector3 (hitPoint.x + (x * spacing), 0.1f, hitPoint.z + (y * spacing));
					UnitScript u = hitColliders [i].GetComponent<UnitScript> ();
					u.hitPoint = Point;
					u.collecting = false;
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
			mousePosition1 = Input.mousePosition;
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
		if (Input.GetMouseButtonUp(0)){
			if (hitColliders.Length != 0){
				source.PlayOneShot(ready, 3.0f);
			}
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
	
	void OnGUI()
    {
        if(Input.GetMouseButton (0))
        {
            // Create a rect from both mouse positions
			var rect = BoxSelectionScript.GetScreenRect(mousePosition1, Input.mousePosition);
            BoxSelectionScript.DrawScreenRect( rect, new Color( 0.8f, 0.8f, 0.95f, 0.25f ) );
        }
    }
	public static Rect GetScreenRect( Vector3 screenPosition1, Vector3 screenPosition2 )
	{
    	// Move origin from bottom left to top left
    	screenPosition1.y = Screen.height - screenPosition1.y;
    	screenPosition2.y = Screen.height - screenPosition2.y;
    	// Calculate corners
    	var topLeft = Vector3.Min( screenPosition1, screenPosition2 );
    	var bottomRight = Vector3.Max( screenPosition1, screenPosition2 );
    	// Create Rect
    	return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
	}
	public static void DrawScreenRect( Rect rect, Color color )
    {
        GUI.color = color;
        GUI.DrawTexture( rect, WhiteTexture );
        GUI.color = Color.white;
    }
	
}


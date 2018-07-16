using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class boss : MonoBehaviour {

	//private List <Vector3> usedPositions = new List<Vector3> ();
	private int callCount = 0; //max = 4
	private int monkCount = 0;
	private int hp = 35;
	private bool canAttack = false;
	private float hitpoint = 35.0f;
	private float maxHitpoint = 35.0f;
	private GameObject bar;
	private GameObject healthbar;

	
	public Image currentHealthBar;
	public Text sText;
	public GameObject player;
	private GameObject defense;
	private GameObject boss_dead_audio;
	private GameObject boss_dead_blood;

	//发射子弹
	public Vector3 bulletOffSet = new Vector3(0,0.5f,0);
	public GameObject bulletPrefab;
	Transform target;
	public float fireDelay = 1.5f;
	float coolDownTimer = 0;
	public float rotSpeed = 90f;

	void InitItem(string objName, float x, float y, float scalex, float scaley){
		if (objName != null) {
			GameObject temp = Instantiate (Resources.Load (objName), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
			temp.transform.localScale = new Vector3 (scalex, scaley, 1.0f);
			if(objName != "boss_send_monk"){
				monkCount++;
			}
		}
	}
	
	private void UpdateHealthBar(){
		float ratio = hitpoint / maxHitpoint;
		GameObject.Find("xiaoCanvas/bossblood").GetComponent<Image>().rectTransform.localScale = new Vector3 (ratio, 1, 1);
		Debug.Log("baifenbi " + ratio);
		//ratioText.text = (ratio * 100).ToString () + '%';
	}
	
	private void ShowStatus(string info) {
		sText.text = info;
		Invoke ("HideStatus", 5.0f);
	}

	private void HideStatus() {
		sText.text = "";
		//Debug.Log ("Hide Status: " + statusText.text);
	}

	public void monkDie(){
		GetComponent<AudioSource>().Play ();
		monkCount--;
		player.SendMessage ("ShowStatus", monkCount + " little monks left");
		Debug.Log (monkCount);

		if (monkCount == 0){
			Debug.Log ("Can attack the boss now");
			if (defense != null) {
				Destroy (defense);
				defense = null;
			}
			canAttack = true;
			player.SendMessage ("ShowStatus", " Attack the boss now!");
		} else if (monkCount < 0) {
			monkCount = 0;
		}
	}

	void parseData(string fileName){
		//Call InitItem to generate items
//		using (StreamReader r = new StreamReader("./Assets/Level_Design/" + fileName + ".json"))
//		{

//			string json = r.ReadToEnd();
		string json = "";
		switch (fileName) {
		case "boss1":
			json = "{ \n\t  \"monk1\": [\n\t\t\t  {\n\t\t\t\t\t  \"x\": -5.31,\n            \"y\": 2.13,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n          {\n\t\t\t\t\t  \"x\": 2.58,\n            \"y\": 1.73,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": 6.89,\n            \"y\": -3.07,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -7.46,\n            \"y\": -2.81,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -0.68,\n            \"y\": -0.56,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t}\n\t\t],\n    \"monk_walking\": [\n\n        {\n            \"x\": -5.97,\n            \"y\": -0.32,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        }\n    ],\n\t  \"monk_walkingp\": [\n\t\t\t\n        {\n            \"x\": -2.33,\n            \"y\": -3.83,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        }\n    ],\n\t  \"monk_walkingpp\": [\n\t\t\t\n        {\n            \"x\": 4.77,\n            \"y\": 2.81,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        }\n    ],\n\t  \"monk_chasing\": [\n        {\n            \"x\": -3.23,\n            \"y\": 0.02,\n            \"scaleX\": 0.8,\n            \"scaleY\": 0.8\n        }\n    ],\n\t\"boss_send_monk\": [\n\t\t {\n\t\t\t\t\t  \"x\": -5.21,\n            \"y\": 2.23,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n          {\n\t\t\t\t\t  \"x\": 2.48,\n            \"y\": 1.83,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": 6.79,\n            \"y\": -3.17,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -7.56,\n            \"y\": -2.71,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -0.78,\n            \"y\": -0.56,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n        {\n\t\t\t\"x\": -3.13,\n            \"y\": -0.08,\n            \"scaleX\": 0.8,\n            \"scaleY\": 0.8\n        },\n        {\n            \"x\": -5.97,\n            \"y\": -0.32,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        },\n\t\t        {\n            \"x\": -2.33,\n            \"y\": -3.83,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        },\n\t\t   {\n            \"x\": 4.77,\n            \"y\": 2.81,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        }\n    ]  \n}";
			break;
		case "boss2":
			json = "{ \n\t  \"monk1\": [\n\t\t\t  {\n\t\t\t\t\t  \"x\": -9.43,\n            \"y\": 3.11,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n          {\n\t\t\t\t\t  \"x\": 9.74,\n            \"y\": 2.81,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": 9.77,\n            \"y\": -3.53,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -2.65,\n            \"y\": -1.07,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -8.11,\n            \"y\": -0.02,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": 3.36,\n            \"y\": -1.5,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t}\n\t\t],\n    \"monk_walkingpoo\": [\n\n        {\n            \"x\": -9.98,\n            \"y\": -1.39,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        }\n    ],\n\t  \"monk_walkingpo\": [\n\t\t\t\n        {\n            \"x\": 0.53,\n            \"y\": -2.94,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        }\n    ],\n\t  \"monk_chasing\": [\n        {\n            \"x\": 2.21,\n            \"y\": -0.25,\n            \"scaleX\": 0.8,\n            \"scaleY\": 0.8\n        }\n    ],\n\t\"boss_send_monk\":[\n\t  {\n\t\t\t\t\t  \"x\": -9.43,\n            \"y\": 3.11,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n          {\n\t\t\t\t\t  \"x\": 9.74,\n            \"y\": 2.81,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": 9.77,\n            \"y\": -3.53,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -2.65,\n            \"y\": -1.07,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -8.11,\n            \"y\": -0.02,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": 3.36,\n            \"y\": -1.5,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\n\t\t {\n            \"x\": -9.98,\n            \"y\": -1.39,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        },\n\t\t {\n            \"x\": 0.53,\n            \"y\": -2.94,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        },\n\t\t       {\n            \"x\": 2.21,\n            \"y\": -0.25,\n            \"scaleX\": 0.8,\n            \"scaleY\": 0.8\n        }\n\t\t]  \n}";
			break;
		case "boss3":
			json = "{ \n\t  \"monk1\": [\n\t\t\t  {\n\t\t\t\t\t  \"x\": -5.62,\n            \"y\": -1.53,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n          {\n\t\t\t\t\t  \"x\": 5.03,\n            \"y\": 3.44,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -2.06,\n            \"y\": 1.78,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t}\n\t\t],\n    \"monk_walkingpu\": [\n\n        {\n            \"x\": 5.47,\n            \"y\": 1.49,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        }\n    ],\n\t  \"monk_walkingpuu\": [\n\t\t\t\n        {\n            \"x\": -3.47,\n            \"y\": -3.34,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        }\n    ],\n\t  \"monk_chasing\": [\n        {\n            \"x\": 4.54,\n            \"y\": -3.63,\n            \"scaleX\": 0.8,\n            \"scaleY\": 0.8\n        }\n    ],\n\t\"boss_send_monk\": [\n\t\n\t\t {\n\t\t\t\t\t  \"x\": -5.62,\n            \"y\": -1.53,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n          {\n\t\t\t\t\t  \"x\": 5.03,\n            \"y\": 3.44,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\t\t\t    {\n\t\t\t\t\t  \"x\": -2.06,\n            \"y\": 1.78,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n\t\t\t\t},\n\n\n        {\n            \"x\": 5.47,\n            \"y\": 1.49,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        },\n\t\t     {\n            \"x\": -3.47,\n            \"y\": -3.34,\n            \"scaleX\": 1,\n            \"scaleY\": 1\n        },\n\n\t\t      {\n            \"x\": 4.54,\n            \"y\": -3.63,\n            \"scaleX\": 0.8,\n            \"scaleY\": 0.8\n        }\n\n\n\t\t] \n}";
			break;
		default:
			break;
		}
			JSONObject jobj = new JSONObject (json);
			for (int i = 0; i < jobj.list.Count; i++) {
				//GameObject t = objMatch (jobj.keys [i]);
				//Debug.Log (t);
				if (jobj.list [i].type == JSONObject.Type.OBJECT) {
					float x = 0.0f, y = 0.0f, scalex = 0.0f, scaley = 0.0f;
					for(int j = 0; j < jobj.list [i].list.Count; j++){
						string key = (string)jobj.list [i].keys[j];
						JSONObject k = (JSONObject)jobj.list [i].list[j];
						if (key == "x") {
							x = k.n;
						} else if (key == "y") {
							y = k.n;
						} else if (key == "scaleX") {
							scalex = k.n;
						} else if (key == "scaleY") {
							scaley = k.n;
						}
					}
					InitItem (jobj.keys [i], x, y, scalex, scaley);
				} else if (jobj.list [i].type == JSONObject.Type.ARRAY) {
					foreach(JSONObject ao in jobj.list [i].list){
						float x = 0.0f, y = 0.0f, scalex = 0.0f, scaley = 0.0f;
						for(int j = 0; j < ao.list.Count; j++){
							string key = (string)ao.keys[j];
							JSONObject k = (JSONObject)ao.list[j];
							if (key == "x") {
								x = k.n;
							} else if (key == "y") {
								y = k.n;
							} else if (key == "scaleX") {
								scalex = k.n;
							} else if (key == "scaleY") {
								scaley = k.n;
							}
						}
						InitItem (jobj.keys [i], x, y, scalex, scaley);
					}
				}
			}
//		}
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating ("fangxiaobing", 5.0f, 40.0f);
		bar = Instantiate (Resources.Load ("fahai_bar"), new Vector3 (7.5f, 2.2f, 0f), Quaternion.identity) as GameObject;
		bar.transform.SetParent(GameObject.Find ("xiaoCanvas").transform);
		healthbar = Instantiate (Resources.Load ("boss_lvl"), new Vector3 (6.5f, 2.22f, 0f), Quaternion.identity) as GameObject;
		healthbar.name = "bossblood";
		healthbar.transform.SetParent(GameObject.Find ("xiaoCanvas").transform);
		player = GameObject.Find ("head");
		player.SendMessage ("changeStatusColor", 1);
		//sText = GameObject.Find ("Canvas/HealthSet/StatusText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		//if(canAttack == true){
			if (target == null) {
				GameObject go= GameObject.Find ("head");
				if (go != null) {
					target = go.transform;
				}
			}
			if (target == null)
				return;
			Vector3 dir = target.position - transform.position;
			dir.Normalize ();
			float zAngle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 90f;
			Quaternion desiredRot = Quaternion.Euler (0, 0, zAngle);
		  //Quaternion curRotation = Quaternion.RotateTowards (transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
			transform.rotation =  Quaternion.RotateTowards (transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
			coolDownTimer -= Time.deltaTime;
			if (coolDownTimer <= 0) {
				coolDownTimer = fireDelay;
				Vector3 offset = transform.rotation * bulletOffSet;
				GameObject bulletGo = (GameObject)Instantiate (bulletPrefab, transform.position + offset, transform.rotation);
				bulletGo.layer = gameObject.layer;
			}
		//}
	}

	void fangxiaobing(){
		canAttack = false;
		if (callCount == 0) {
			parseData ("boss1");
			Debug.Log ("Gong you" + monkCount + "ge he shang");
			defense =  Instantiate (Resources.Load ("fahai_defense"), new Vector3 (7.7f, 2.3f, 0f), Quaternion.identity) as GameObject;
			callCount++;
		} else if (callCount == 1) {
			fireDelay = 1.0f;
			parseData ("boss2");
			if (defense == null) {
				defense =  Instantiate (Resources.Load ("fahai_defense"), new Vector3 (7.7f, 2.3f, 0f), Quaternion.identity) as GameObject;
			}
			callCount++;
		} else if (callCount == 2) {
			parseData ("boss3");
			if (defense == null) {
				defense =  Instantiate (Resources.Load ("fahai_defense"), new Vector3 (7.7f, 2.3f, 0f), Quaternion.identity) as GameObject;
			}
			callCount++;
		} else if (callCount == 3) {
			callCount = 0;
			//Destroy (defense);
		}
	}
	
	void OnTriggerStay2D(Collider2D other){
		if (other.name.StartsWith("bullet")) {
			Destroy (other.gameObject);
			if(canAttack == true){
				hp--;
				hitpoint -= 1.0f;
				UpdateHealthBar();
				Debug.Log("sheng xia " + hp + "dian xue");
				if(hp == 0){
					//GetComponent<AudioSource>().Play ();
					boss_dead_audio = Instantiate (Resources.Load ("boss_death_audio"), new Vector3 (7.5f, 2.2f, 0f), Quaternion.identity) as GameObject;
					boss_dead_blood = Instantiate (Resources.Load ("blood_monk"), new Vector3 (7.5f, 2.2f, 0f), Quaternion.identity) as GameObject;

					Destroy(gameObject);
					Destroy(bar);
					Destroy(healthbar);
				}
				if(hitpoint < 0.0f){
					hitpoint = 0.0f;
				}
			}
		} 
	}
}

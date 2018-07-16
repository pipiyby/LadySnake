using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour {

	Snake_head head;
	public Button btUp;
	public Button btDown;
	public Button btLeft;
	public Button btRight;
	public Button btAttack;
	public Button btSupAttack;
	public Button btMagnet;
	public Button btAddHealth;
	public Button btShield;


	// Use this for initialization
	void Start () {
		head = GameObject.Find ("head").GetComponent<Snake_head>();

		btUp = GameObject.Find ("ButtonUp").GetComponent<Button>();
		btUp.onClick.AddListener(MoveUp);

		btDown = GameObject.Find ("ButtonDown").GetComponent<Button>();
		btDown.onClick.AddListener(MoveDown);

		btLeft = GameObject.Find ("ButtonLeft").GetComponent<Button>();
		btLeft.onClick.AddListener(MoveLeft);

		btRight = GameObject.Find ("ButtonRight").GetComponent<Button>();
		btRight.onClick.AddListener(MoveRight);

		btAttack = GameObject.Find ("ButtonAttack").GetComponent<Button>();
		btAttack.onClick.AddListener(Attack);

		btSupAttack = GameObject.Find ("ButtonSupperAttack").GetComponent<Button>();
		btSupAttack.onClick.AddListener(SupperAttack);

		btShield = GameObject.Find ("ButtonShield").GetComponent<Button>();
		btShield.onClick.AddListener(Shield);

		btAddHealth = GameObject.Find("ButtonAddHealth").GetComponent<Button>();
		btAddHealth.onClick.AddListener(AddHealth);

		btMagnet = GameObject.Find("ButtonMagnet").GetComponent<Button>();
		btMagnet.onClick.AddListener (Magnet);


		//disBtSupAttack = GameObject.Find("ButtonMagnet").GetComponent<Button>();
	}

	void initVis() {
		btSupAttack.gameObject.SetActive (false);

		btAddHealth.gameObject.SetActive (false);
		btMagnet.gameObject.SetActive (false);
	}


	void MoveUp () {
		head.vertical2 = -1;
	}

	void MoveDown () {
		head.vertical2 = 1;
	}

	void MoveLeft () {
		head.horizonal2 = -1;
	}

	void MoveRight () {
		head.horizonal2 = 1;
	}

	void Attack () {
		head.btAttack = true;
	}

	void SupperAttack() {
		head.btSupAttack = true;
	}

	void Shield() {
		head.btShield = true;
	}

	void Magnet() {
		head.btMagnet = true;
	}

	void AddHealth() {
		head.btAddHealth = true;
	}

	// Update is called once per frame
//	void Update () {
//		if (head.shieldCount > 0) {
//			btShield.gameObject.SetActive (true);
//		} else {
//			btShield.gameObject.SetActive (false);
//		}
//
//		if (head.magnetCount > 0) {
//			btMagnet.gameObject.SetActive (true);
//		} else {
//			btMagnet.gameObject.SetActive (false);
//		}
//
//		if (head.treatCount > 0) {
//			btAddHealth.gameObject.SetActive (true);
//		} else {
//			btAddHealth.gameObject.SetActive (false);
//		}
//
//		if (head.aggressCount > 0) {
//			btSupAttack.gameObject.SetActive (true);
//		} else {
//			btSupAttack.gameObject.SetActive (false);
//		}
//	}
}

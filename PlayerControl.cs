using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerControl : NetworkBehaviour {

	//public float speed;
	public GameObject punchTrigger;

	public Canvas endScreen;
	public Text endText;
	public Button endButton;

	//[SyncVar]
	public Light aura;

	private CharacterController cc;
	private Animator anim;
	private PlayerHealth health;

	//Attributes
	public float damage = 5.0f;
	public float defense = 2.0f;
	public float speed = 1.1f;
	public float agility = 1.0f;

	private bool dead;
	private int deathSet;
	private bool canPunch;
	private bool blocking;
	private bool canControl;

	private bool isLocal;
	private bool fighting;
	private bool startGame;
	private bool gameOver;


	public int test = 1;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<CharacterController> ().enabled = true;
		cc = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
		health = GetComponent<PlayerHealth> ();

		endScreen = GameObject.FindGameObjectWithTag ("end").GetComponent<Canvas>();
		endText = GameObject.FindGameObjectWithTag ("endText").GetComponent<Text>();
		endButton = GameObject.FindGameObjectWithTag ("endButton").GetComponent<Button>();
		endScreen.gameObject.SetActive (false);
		endText.gameObject.SetActive (false);
		endButton.gameObject.SetActive (false);

		dead = false;

		canPunch = true;
		blocking = false;

		fighting = false;

		startGame = false;
		canControl = true;
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate(){
		if (!dead && canControl && fighting) {
			if(Input.GetMouseButton(1)){
				anim.SetBool ("Block", true);
				blocking = true;
			} else if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) { //Moving forward
				anim.SetBool ("Backward", false);
				anim.SetBool ("Block", false);
				blocking = false;
				if (Input.GetKey (KeyCode.LeftShift)) { //Running
					anim.SetBool ("Walking", false);
					anim.SetBool ("Running", true);
					cc.SimpleMove (transform.forward * speed * 3);
					canPunch = false;
				} else { //Walking
					anim.SetBool ("Walking", true);
					anim.SetBool ("Running", false);
					cc.SimpleMove (transform.forward * speed);
					canPunch = true;
				}
			} else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) { //Walking Backward
				anim.SetBool ("Walking", false);
				anim.SetBool ("Backward", true);
				anim.SetBool ("Running", false);
				anim.SetBool ("Block", false);
				blocking = false;
				cc.SimpleMove (-1 * transform.forward * speed);
				canPunch = true;
			} else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) { //Strafing Right
				canPunch = false;
				anim.SetBool ("Walking", false);
				anim.SetBool ("Backward", false);
				anim.SetBool ("Running", false);
				anim.SetBool ("Right", true);
				anim.SetBool ("Left", false);
				anim.SetBool ("Block", false);
				blocking = false;
				cc.SimpleMove (transform.right * speed);
			} else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) { //Strafing Left
				canPunch = false;
				anim.SetBool ("Walking", false);
				anim.SetBool ("Backward", false);
				anim.SetBool ("Running", false);
				anim.SetBool ("Right", false);
				anim.SetBool ("Left", true);
				anim.SetBool ("Block", false);
				blocking = false;
				cc.SimpleMove (-1 * transform.right * speed);
			} else { //Nothing
				cc.SimpleMove (Vector3.zero);
				anim.SetBool ("Walking", false);
				anim.SetBool ("Running", false);
				anim.SetBool ("Backward", false);
				anim.SetBool ("Right", false);
				anim.SetBool ("Left", false);
				anim.SetBool ("Block", false);
				blocking = false;
				canPunch = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape) && canControl) {
			if (Cursor.visible) {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			} else {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		if (health.getDead() && deathSet == 0) {
			//StartCoroutine (death ());
			anim.SetBool ("Death", true);
			deathSet++;
		}
	}

	public void takeHit(float d){
		if (!blocking) {
			StartCoroutine (flinch ());
			if (d - defense <= 0) {
				health.TakeDamage (1);
			} else {
				health.TakeDamage (d - defense);
			}
		} else {
			if ((d / 2) - defense <= 0) {
				health.TakeDamage (1);
			} else {
				health.TakeDamage ((d / 2) - defense);
			}
		}
	}

	private IEnumerator flinch(){
		anim.SetBool ("Flinch", true);
		yield return new WaitForSeconds (0.08f);
		anim.SetBool ("Flinch", false);
	}

	private IEnumerator death(){
		anim.SetBool ("Death", true);
		yield return new WaitForSeconds (0.05f);
		anim.SetBool ("Death", false);
		Debug.Log ("DEAD");
	}

	public float getDamage(){
		return damage;
	}

	public bool getDead(){
		return health.getDead();
	}

	public void setDeath (bool b){
		dead = b;
	}

	public bool getPunch(){
		return canPunch;
	}

	public bool getCanControl(){
		return canControl;
	}

	public void setUpPlayer(int str, int def, int spd, int agy){
		//Balancing here to establish the players strength, defense, speed, and agility
		damage += str;
		defense *= (def / 2) + 1;
		speed += spd / 2;
		agility += agy / 6;
	}

	public bool isTheLocalPlayer(){
		return isLocal;
	}

	public void setLocalPlayer(bool b){
		isLocal = b;
	}

	public bool fightStart(){
		return fighting;
	}

	public bool getStart(){
		return startGame;
	}

	public void setStart(bool b){
		startGame = b;
	}

	public void startFight(){
		fighting = true;
		Camera.main.GetComponent<CameraController> ().SetUp ();
	}

	public float getAgility(){
		return (1/agility)/5;
	}

	public void endGame(){
		gameOver = true;
		canControl = false;
		health.deactiveHealth ();
		endScreen.gameObject.SetActive (true);
		endText.gameObject.SetActive (true);
		endButton.gameObject.SetActive (true);
		if (dead || health.currentHealth <= 0) {
			endText.text = "YOU LOSE";
		} else {
			endText.text = "YOU WIN";
		}
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}

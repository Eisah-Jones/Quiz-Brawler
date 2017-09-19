using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AttackScript : NetworkBehaviour {

	public GameObject punchTrigger;


	private Animator anim;
	private PlayerControl pc;
	private bool canAttack;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		pc = GetComponent<PlayerControl> ();

		canAttack = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && pc.getCanControl() && pc.fightStart()) {
			attack ();
		}
	
	}
		
	protected void attack(){
		if (!canAttack || pc.getDead() || !pc.getPunch()) { //Make sure that the player is able to attack
			return;
		}
		StartCoroutine (attackAnimation ());
		//StartCoroutine (attackTrigger ());
		Cmdattack ();
	}

	[Command]
	void Cmdattack(){
		Rpcattack ();
	}

	[ClientRpc]
	void Rpcattack(){
		StartCoroutine (attackTrigger ());
	}

	private IEnumerator attackTrigger(){
		yield return new WaitForSeconds (0.25f);
		punchTrigger.SetActive (true);
		yield return new WaitForSeconds (0.15f);
		punchTrigger.SetActive (false);
	}

	private IEnumerator attackAnimation(){
		float attackWait = pc.getAgility ();
		canAttack = false;
		anim.SetBool ("Punching", true);
		yield return new WaitForSeconds (attackWait/2);
		anim.SetBool ("Punching", false);
		yield return new WaitForSeconds (attackWait/2);
		canAttack = true;
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

	public RectTransform healthBar;
	public const float maxHealth = 100;
	public Canvas healthCanvas;

	[SyncVar]
	private bool death;

	[SyncVar]
	public float currentHealth = maxHealth;

	void Start(){
		death = false;
		healthBar = GameObject.FindGameObjectWithTag ("healthBar").GetComponent<RectTransform>();
		healthCanvas = GameObject.FindGameObjectWithTag ("healthCanvas").GetComponent<Canvas>();
	}

	public void TakeDamage(float amount){
		currentHealth -= amount;
		if (currentHealth <= 0) {
			currentHealth = 0;
			death = true;
			GetComponent<CharacterController> ().enabled = false;
			Cmddeactivate ();
			GetComponent<fightSensor> ().dead = true;
			healthBar.gameObject.SetActive (false);
		}

		healthBar.sizeDelta = new Vector2 (currentHealth*2.45f, healthBar.sizeDelta.y);
	}

	[Command]
	private void Cmddeactivate(){
		Rpcdeactive ();
	}

	[ClientRpc]
	private void Rpcdeactive(){
		GetComponent<CharacterController> ().enabled = false;
		GetComponent<fightSensor> ().dead = true;
	}

	public bool getDead(){
		return death;
	}

	public void deactiveHealth(){
		healthCanvas.gameObject.SetActive (false);
	}
}

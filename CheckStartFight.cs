using UnityEngine;
using System.Collections;

public class CheckStartFight : MonoBehaviour {

	public Canvas upgrade;

	public bool ready;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("checkIfReady", 1f, 1f);
		ready = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void checkIfReady(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		int count = 0;
		if (!ready) {
			foreach (GameObject p in players) {
				if (p.GetComponent<fightSensor> ().getStart ()) {
					++count;
				}
			}
			if (count >= players.Length) {
				foreach (GameObject p in players) {
					p.GetComponent<PlayerControl> ().startFight ();
				}
				upgrade.gameObject.SetActive (false);
				ready = true;
			}
		} else if (ready) {
			foreach (GameObject p in players) {
				if (p.GetComponent<fightSensor> ().dead == true) {
					++count;
				}
			}
			if (count >= players.Length-1) {
				foreach (GameObject p in players) {
					if (p.GetComponent<PlayerControl> ().isTheLocalPlayer ()) {
						p.GetComponent<PlayerControl> ().endGame ();
					}
				}
			}
		}
	}
}

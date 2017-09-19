using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PunchTrigger : NetworkBehaviour {

	public GameObject parent;

	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	}
		

	void OnTriggerEnter (Collider other){
		try{
			if(other.gameObject != parent){
				other.gameObject.GetComponent<PlayerControl>().takeHit(parent.GetComponent<PlayerControl>().getDamage());
			}
		} catch{}
	}
}

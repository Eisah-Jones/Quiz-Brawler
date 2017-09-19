using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class fightSensor : NetworkBehaviour {

	[SyncVar]
	public bool readyToFight;

	[SyncVar]
	public bool dead;

	// Use this for initialization
	void Start () {
		readyToFight = false;
		dead = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[Command]
	public void CmdchangeFight(Color c, int str, int def, int spd, int agy){
		RpcchangeFight (c, str, def, spd, agy);
	}

	[ClientRpc]
	public void RpcchangeFight(Color c, int str, int def, int spd, int agy){
		readyToFight = true;
		GetComponentInChildren<Light>().color = c;
		GetComponent<PlayerControl> ().setUpPlayer(str, def, spd, agy);
	}

	[Command]
	public void CmdsetDead(){
		RpcsetDead();
	}

	[ClientRpc]
	public void RpcsetDead(){
		dead = true;
	}

	public bool getStart(){
		return readyToFight;
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LocalPlayer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GetComponent<PlayerControl> ().enabled = true;
			GetComponent<PlayerControl> ().setLocalPlayer (true);
			GetComponent<AttackScript> ().enabled = true;
			//Camera.main.GetComponent<CameraController> ().SetUp ();
			Invoke ("postSetup", 1f);
		}
	}

	void postSetup(){
		Camera.main.GetComponent<CameraController> ().SetUp ();
	}
}

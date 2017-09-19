using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {

	public GameObject target;
	public float rotateSpeed = 5;

	private Vector3 offset;
	private PlayerControl pc;

	// Use this for initialization
	void Start () {
		target = null;
	}

	// Update is called once per frame
	void LateUpdate () {
		if (target != null && !pc.getDead() && pc.fightStart() && pc.getCanControl()) {
			float horizontal = Input.GetAxis ("Mouse X") * rotateSpeed;
			target.transform.Rotate (0, horizontal, 0);

			float desiredAngle = target.transform.eulerAngles.y;
			Quaternion rotation = Quaternion.Euler (0, desiredAngle, 0);
			transform.position = target.transform.position - (rotation * offset);

			transform.LookAt (new Vector3 (target.transform.position.x, (target.transform.position.y + 1.4f), target.transform.position.z));
		}
	}

	public void SetUp(){
		findTarget ();
		pc = target.GetComponent<PlayerControl> ();
		if (pc.fightStart ()) {
			transform.position = new Vector3 (target.transform.position.x - 0.3171f, target.transform.position.y + 2.11f, target.transform.position.z - 2.51f);
			offset = target.transform.position - transform.position;
		} else {
			transform.position = new Vector3 (target.transform.position.x - 1.42f, target.transform.position.y + 1f, target.transform.position.z + 2.43f);
			transform.rotation = new Quaternion (transform.rotation.x, 253, transform.rotation.z, transform.rotation.w);
			offset = target.transform.position - transform.position;
		}
	}

	public void findTarget(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject p in players) {
			if (p.GetComponent<PlayerControl> ().isTheLocalPlayer ()) {
				target = p;
				return;
			}
		}
	}

	public void MainMenu(){
		//Network.Disconnect ();
		GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<NetworkLobbyManager>().SendReturnToLobby();
	}
}

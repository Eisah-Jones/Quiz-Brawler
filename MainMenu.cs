using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Canvas main;
	public Canvas controls;
	public Canvas howTo;

	// Use this for initialization
	void Start () {
		main.gameObject.SetActive (true);
		controls.gameObject.SetActive (false);
		howTo.gameObject.SetActive (false);
	}
	
	public void playButton(){
		SceneManager.LoadScene ("Lobby");
	}

	public void controlButton(){
		main.gameObject.SetActive (false);
		controls.gameObject.SetActive (true);
		howTo.gameObject.SetActive (false);
	}

	public void backButton(){
		main.gameObject.SetActive (true);
		controls.gameObject.SetActive (false);
		howTo.gameObject.SetActive (false);
	}

	public void howToButton(){
		howTo.gameObject.SetActive (true);
		main.gameObject.SetActive (false);
		controls.gameObject.SetActive (false);
	}
}

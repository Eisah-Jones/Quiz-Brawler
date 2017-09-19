using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class Quiz : NetworkBehaviour {

	public Canvas quiz;
	public Canvas preamble;
	public Canvas upgrade;

	public Text scoreText;
	public Text timerText;
	public Text auraText;
	public Text waiting;

	public Text q;

	public Button a1;
	public Button a2;
	public Button a3;
	public Button a4;

	public Button t;
	public Button f;

	public Button ready;

	public AudioClip wrong;
	public AudioClip right;

	public GameObject[] strengthBars;
	public GameObject[] defenseBars;
	public GameObject[] speedBars;
	public GameObject[] agilityBars;

	[SyncVar]
	private bool startGame = false;

	private bool timerRepeat = true;

	private int quizIndex = 0;
	private int score = 0;
	private int timerSecs = 30;

	private int strength = 0;
	private int defense = 0;
	private int speed = 0;
	private int agility = 0;

	private Light aura;



	private GameObject player;

	struct Question{
		public bool isTorF;

		public string q;

		public string a1;
		public string a2;
		public string a3;
		public string a4;

		public string answer;
	};

	Question current;
	Question TF;
	Question M;
	Question[] questionList = new Question[13];

	// Use this for initialization
	void Start () {
		loadQuestions ();
		current = questionList [quizIndex];
		updateScore ();
		preamble.gameObject.SetActive (true);
		quiz.gameObject.SetActive (false);
		upgrade.gameObject.SetActive (false);
		Invoke ("startQuiz", 5f);
	}

	public static void ShuffleArray<T>(T[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i);
			T tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void loadQuestions(){
		int tempIndex = 0;
		//string[] file = File.ReadAllLines ("questions.txt");
		TextAsset qs = Resources.Load("questions") as TextAsset;
		string[] file = qs.text.Split("\n"[0]);
		for (int i = 0; tempIndex < 13;) {
			Question tempQ;
			if (file [i].Trim() == "MC") {
				tempQ.isTorF = false;
				tempQ.q = file [i+1].Trim();
				tempQ.a1 = file [i+2].Trim();
				tempQ.a2 = file [i+3].Trim();
				tempQ.a3 = file [i+4].Trim();
				tempQ.a4 = file [i+5].Trim();
				tempQ.answer = file [i+6].Trim();
				i += 7;
				questionList [tempIndex] = tempQ;
			} else if (file [i].Trim() == "TF"){
				tempQ.isTorF = true;
				tempQ.q = file [i+1].Trim();
				tempQ.a1 = null;
				tempQ.a2 = null;
				tempQ.a3 = null;
				tempQ.a4 = null;
				tempQ.answer = file [i+2].Trim();
				i += 3;
				questionList [tempIndex] = tempQ;
			}
			++i;
			++tempIndex;
		}

		ShuffleArray (questionList);
	}

	public void startQuiz (){
		preamble.gameObject.SetActive (false);
		quiz.gameObject.SetActive (true);
		setQuestion ();
		InvokeRepeating ("changeTimer", 1f, 1f);
	}

	private void changeTimer(){
		if (timerRepeat) {
			timerSecs -= 1;
			if (timerSecs < 0) {
				setPlayer ();
				Camera.main.GetComponent<CameraController> ().SetUp ();
				timerSecs = 0;
				quiz.gameObject.SetActive (false);
				upgrade.gameObject.SetActive (true);
				setAuraPoints ();
				setAura ();
				waiting.gameObject.SetActive (false);
				timerRepeat = false;
			} else {
				timerText.text = timerSecs.ToString () + " seconds"; 
			}
		}
	}

	public void setQuestion(){
		if (current.isTorF) { //Means that the question is true or false
			a1.gameObject.SetActive(false);
			a2.gameObject.SetActive(false);
			a3.gameObject.SetActive(false);
			a4.gameObject.SetActive(false);
			t.gameObject.SetActive (true);
			f.gameObject.SetActive (true);
			q.text = current.q;
		} else {
			a1.gameObject.SetActive(true);
			a2.gameObject.SetActive(true);
			a3.gameObject.SetActive(true);
			a4.gameObject.SetActive(true);
			t.gameObject.SetActive (false);
			f.gameObject.SetActive (false);
			q.text = current.q;
			a1.GetComponentInChildren<Text>().text = current.a1;
			a2.GetComponentInChildren<Text>().text = current.a2;
			a3.GetComponentInChildren<Text>().text = current.a3;
			a4.GetComponentInChildren<Text>().text = current.a4;
		}
	}

	private void updateScore(){
		if (score < 0) {
			score = 0;
		}
		scoreText.text = score.ToString ();
	}

	private void nextQuestion(){
		try{
			current = questionList[++quizIndex];
			setQuestion ();
		} catch {
			timerSecs = 0;
		}
	}

	public void pickTrue(){
		if (current.answer == "True") {
			score += 1;
			Right ();
		} else {
			score -= 1;
			Wrong ();
		}
		updateScore ();
		nextQuestion ();
	}

	public void pickFalse(){
		if (current.answer == "False") {
			score += 1;
			Right ();
		} else {
			score -= 1;
			Wrong ();
		}
		updateScore ();
		nextQuestion ();
	}

	public void pickA1(){
		if (current.answer == a1.GetComponentInChildren<Text>().text) {
			score += 2;
			Right ();
		} else {
			score -= 1;
			Wrong ();
		}
		updateScore ();
		nextQuestion ();
	}

	public void pickA2(){
		if (current.answer == a2.GetComponentInChildren<Text>().text) {
			score += 2;
			Right ();
		} else {
			score -= 1;
			Wrong ();
		}
		updateScore ();
		nextQuestion ();
	}

	public void pickA3(){
		if (current.answer == a3.GetComponentInChildren<Text>().text) {
			score += 2;
			Right ();
		} else {
			score -= 1;
			Wrong ();
		}
		updateScore ();
		nextQuestion ();
	}

	public void pickA4(){
		if (current.answer == a4.GetComponentInChildren<Text>().text) {
			score += 2;
			Right ();
		} else {
			score -= 1;
			Wrong ();
		}
		updateScore ();
		nextQuestion ();
	}

	public int getScore(){
		return score;
	}

	public void setAura(){
		setPlayer ();
		aura = player.GetComponentInChildren<Light>();
	}

	public void setBars(string bar){
		GameObject[] barsToChange;
		int index;
		if (bar == "s") {
			barsToChange = strengthBars;
			index = strength;
		} else if (bar == "d") {
			barsToChange = defenseBars;
			index = defense;
		} else if (bar == "a") {
			barsToChange = agilityBars;
			index = agility;
		} else {
			barsToChange = speedBars;
			index = speed;
		}
			
		for (int i = 0; i < index; i++) {
			barsToChange [i].SetActive (true);
		}
		for (int i = index; i < 6; i++) {
			barsToChange [i].SetActive (false);
		}
			
		setAuraPoints ();
	}

	public void setAuraPoints(){
		auraText.text = score.ToString();
	}

	public void minusStrength(){
		--strength;
		if (strength < 0) {
			strength = 0;
		} else {
			++score;
			setBars ("s");
			aura.color -= new Color (0.17f, 0, 0, 1);
		}
	}

	public void plusStrength(){
		if (score > 0) {
			++strength;
			if (strength > 6) {
				strength = 6;
			} else {
				--score;
				setBars ("s");
				aura.color += new Color (0.17f, 0, 0, 1);
			}
		}
	}

	public void minusDefense(){
		--defense;
		if (defense < 0) {
			defense = 0;
		} else {
			++score;
			setBars ("d");
			aura.color -= new Color (0, 0, 0.17f, 1);
		}
	}

	public void plusDefense(){
		if (score > 0) {
			++defense;
			if (defense > 6) {
				defense = 6;
			} else {
				--score;
				setBars ("d");
				aura.color += new Color (0, 0, 0.17f, 1);
			}
		}
	}

	public void minusAgility(){
		--agility;
		if (agility < 0) {
			agility = 0;
		} else {
			++score;
			setBars ("a");
			aura.color -= new Color (0.17f, 0.153f, 0.0027f, 1);
		}
	}

	public void plusAgility(){
		if (score > 0) {
			++agility;
			if (agility > 6) {
				agility = 6;
			} else {
				--score;
				setBars ("a");
				aura.color += new Color (0.17f, 0.153f, 0.0027f, 1);
			}
		}
	}

	public void minusSpeed(){
		--speed;
		if (speed < 0) {
			speed = 0;
		} else {
			++score;
			setBars ("z");
			aura.color -= new Color (0, 0.17f, 0, 1);
		}
	}

	public void plusSpeed(){
		if (score > 0) {
			++speed;
			if (speed > 6) {
				speed = 6;
			} else {
				--score;
				setBars ("z");
				aura.color += new Color (0, 0.17f, 0, 1);
			}
		}
	}
		
	public void CmdclickReady(){
		waiting.gameObject.SetActive (true);
		ready.gameObject.SetActive (false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		startGame = true;
		setPlayer ();
		player.GetComponent<fightSensor> ().CmdchangeFight (aura.color, strength, defense, speed, agility);
	}

	public void setPlayer(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject p in players) {
			if (p.GetComponent<PlayerControl> ().isTheLocalPlayer ()) {
				player = p;
				return;
			}
		}
	}

	public bool getStartGame(){
		return startGame;
	}

	public void hideUpgrade(){
		upgrade.gameObject.SetActive (false);
	}

	private void Wrong(){
		AudioSource.PlayClipAtPoint (wrong, transform.position);
	}

	private void Right(){
		AudioSource.PlayClipAtPoint (right, transform.position);
	}
}

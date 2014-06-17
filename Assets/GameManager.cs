using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	float timer = 0f;

	public float roundTimer = 20.0f;

	public bool gameRunning = true;

	bool paused = false;

	static GameManager s_instance;

	void Start () {
		GameObject.DontDestroyOnLoad (gameObject);
		if(s_instance == null)
			s_instance = this;
		else
			Destroy(gameObject);
	}

	void Update () {
		if(Application.loadedLevelName == "Main") {
			if(timer < roundTimer) {
				timer += Time.deltaTime;
			} else {
				timer = roundTimer;
				NextRound();
			}
		}
	}

	void OnGUI() {
		if(Application.loadedLevelName == "Main") {
			GUI.Label(new Rect(0.0f, 0.0f, 100.0f, 30.0f), "Time: " + Mathf.Round(roundTimer - timer));
		} else {
			if(GUI.Button(new Rect(Screen.width/2.0f - Screen.width/4.0f, Screen.height/2.0f - Screen.height/4.0f, Screen.width/2.0f, Screen.height/2.0f), "Next level")) {
				Application.LoadLevel("Main");
			}
		}
	}

	public void TogglePause() {
		if (!paused) {
			paused = true;
			gameRunning = false;
		} else {
			paused = false;
			gameRunning = true;
		}
	}
	void NextRound() {
		Application.LoadLevel ("Transition");
		timer = 0.0f;
	}

	public GameManager instance {
		get {
			return s_instance;
		}
	}
}
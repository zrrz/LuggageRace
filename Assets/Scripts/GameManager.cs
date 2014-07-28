using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	float timer = 0f;

	public float roundTimer = 20.0f;

	[System.NonSerialized]
	public bool gameRunning = false;

	bool paused = false;

	static GameManager s_instance;

	public float gridFadeInTime = 2.0f;

	public GUISkin guiSkin;

	void Start () {
		Screen.SetResolution(450, 800, false);
		GameObject.DontDestroyOnLoad (gameObject);
		if(s_instance == null)
			s_instance = this;
		else
			Destroy(gameObject);

		ObjectManager.instance.grayscalePlane.SetActive(false);

		StartCoroutine ("FadeInGrid");
	}

	void Update () {
		if(Application.loadedLevelName == "Main") {
			if(gameRunning) {
				if(timer < roundTimer) {
					timer += Time.deltaTime;
				} else {
					timer = roundTimer;
					NextRound();
				}
			}
		}
	}

	void OnLevelWasLoaded(int level) {
		if(level == 0) {
			gameRunning = false;
			StartCoroutine ("FadeInGrid");
		}
	}

	void OnGUI() {
		if(Application.loadedLevelName == "Main") {
			GUI.Label(new Rect(0.0f, 0.0f, 100.0f, 30.0f), "Time: " + Mathf.Round(roundTimer - timer), guiSkin.label);
		} else {
			if(GUI.Button(new Rect(Screen.width/2.0f - Screen.width/4.0f, Screen.height/2.0f - Screen.height/4.0f, Screen.width/2.0f, Screen.height/4.0f), "Next level")) {
				Application.LoadLevel("Main");
			}
		}
	}

	IEnumerator FadeInGrid() {
		yield return new WaitForSeconds (gridFadeInTime);
		Color t_color = ObjectManager.instance.grid.renderer.material.color;
		while(t_color.a < 1.0f) {
			t_color.a += 0.01f;
			ObjectManager.instance.grid.renderer.material.color = t_color;
			yield return null;
		}
		gameRunning = true;
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

	public static GameManager instance {
		get {
			return s_instance;
		}
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {

	class StoreItem {
		public StoreItem(bool p_bought, float p_price, string p_name) {
			bought = p_bought; price = p_price; name = p_name;
		}
		public bool bought = false;
		public float price = 1f;
		public string name = "Language Pack";
	};

	List<StoreItem> storeItems;

	bool storeOpen = false;
	bool optionsOpen = false;

	GUIStyle style;

	void Start () {
		style = new GUIStyle();
		style.wordWrap = true;
		//style.
		storeItems = new List<StoreItem>();
		storeItems.Add(new StoreItem(false, 2.0f, "French Language Pack"));
		storeItems.Add(new StoreItem(true, 1.0f, "German Language Pack")); 
		storeItems.Add(new StoreItem(true, 1.0f, "Russian Language Pack")); 
		storeItems.Add(new StoreItem(false, 2.0f, "Mongolian Language Pack")); 
		storeItems.Add(new StoreItem(false, 1.0f, "Spanish Language Pack")); 
	}

	void Update () {
	
	}

	void OnGUI() {
		float buttonWidth = 100f, buttonHeight = 100f;
		if(storeOpen) {
			if(GUI.Button(new Rect(0f, 0f, buttonWidth, buttonHeight), "Back")) {
				storeOpen = false;
			}
			float width = 200f;
			GUILayout.Width(width);
			GUILayout.BeginArea(new Rect(Screen.width/2.0f - width/2.0f, Screen.height/2f, width, 200f));
			foreach(StoreItem item in storeItems) {
				if(item.bought)
					GUILayout.Label("$" + item.price + " " + item.name, style);
				else
					GUILayout.Button("$" + item.price + " " + item.name, style);
				GUILayout.Space(10f);

			}
			GUILayout.EndArea();
		} else if(optionsOpen) {
			if(GUI.Button(new Rect(0f, 0f, buttonWidth, buttonHeight), "Back")) {
				optionsOpen = false;
			}
		} else {
			if(Application.loadedLevelName == "Intro") {
				if(GUI.Button(new Rect(Screen.width/2.0f - buttonWidth/2.0f, Screen.height/2.0f - buttonHeight/2.0f, buttonWidth, buttonHeight), "Start Game!")) {
					Application.LoadLevel("Main");
				}
				if(GUI.Button(new Rect(Screen.width - buttonWidth, 0f, buttonWidth, buttonHeight), "Store")) {
					storeOpen = true;
				}
				if(GUI.Button(new Rect(Screen.width - buttonWidth, Screen.height - buttonHeight, buttonWidth, buttonHeight), "Options")) {
					optionsOpen = true;
				}
			} else if(Application.loadedLevelName == "Main") {

			}
		}
	}
}
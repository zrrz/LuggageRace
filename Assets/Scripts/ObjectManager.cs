using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {
	
	static ObjectManager s_instance;

	//[System.NonSerialized]
	public ConveyerBelt conveyerBelt;

	//[System.NonSerialized]
	public ItemGrid itemGrid;

	//[System.NonSerialized]
	public GameObject grayscalePlane;

	public GameObject grid;

	public GameManager gameManager;

	void Start () {
		s_instance = this;

		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>(); //fix to a weird problem

		grid = GameObject.Find ("Grid");

		Color t_color = grid.renderer.material.color;
		t_color.a = 0.0f;
		grid.renderer.material.color = t_color;
	}

	public static ObjectManager instance {
		get {
			return s_instance;
		}
	}
}
using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {
	
	static ObjectManager s_instance;

	[System.NonSerialized]
	public ConveyerBelt conveyerBelt;

	[System.NonSerialized]
	public ItemGrid itemGrid;

	[System.NonSerialized]
	public GameObject grayscalePlane;

	void Start () {
		s_instance = this;
		grayscalePlane = GameObject.Find("GrayscalePlane");
		itemGrid = GameObject.Find("ItemGrid").GetComponent<ItemGrid>();
		conveyerBelt = GameObject.Find("Conveyer Belt").GetComponent<ConveyerBelt>();

		grayscalePlane.SetActive (false);
	}

	public static ObjectManager instance {
		get {
			return s_instance;
		}
	}
}
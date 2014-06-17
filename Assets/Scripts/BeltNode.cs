using UnityEngine;
using System.Collections;

public class BeltNode : MonoBehaviour {
	public GameObject obj;
	public bool on;

	public float speed;

	public int index;

	void Start () {
	
	}

	void Update () {
		if(on) {
			if(ObjectManager.instance.gameManager.gameRunning)
				transform.localPosition += Vector3.right * speed * Time.deltaTime;
		}
	}
}

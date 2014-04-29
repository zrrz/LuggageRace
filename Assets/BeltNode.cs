using UnityEngine;
using System.Collections;

public class BeltNode : MonoBehaviour {
	public GameObject obj;
	public bool on;

	public float speed;

	void Start () {
	
	}

	void Update () {
		if(on)
			transform.localPosition += Vector3.right * speed * Time.deltaTime;
	}
}

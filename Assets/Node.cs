using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
	public GameObject obj;
	//public Node left, right, up, down;
	public int xPos, yPos;

	void Update() {
		if(obj)
			for(int i = 0; i < transform.childCount; i++)
				transform.GetChild(i).renderer.material.color = Color.cyan;
	}
}

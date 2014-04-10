using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyerBelt : MonoBehaviour {

	LinkedList<GameObject> nodes;

	public float speed = 1.0f;

	public GameObject[] objs;

	public GameObject nodePrefab;

	float beltStart;
	float beltEnd;
	float nodeSize;

	public int NumNodes = 10;

	void Start () {
		beltStart = transform.position.x - transform.FindChild("Belt").localScale.x / 2.0f;
		beltEnd = transform.position.x + transform.FindChild("Belt").localScale.x / 2.0f;
		nodeSize = transform.FindChild ("Belt").localScale.x / NumNodes;
		nodes = new LinkedList<GameObject> ();
		for (int i = 0; i < NumNodes; i++) {
			nodes.AddFirst (NewNode(new Vector3(beltStart + (nodeSize * i), 0.0f, -0.5f)));
			SpawnItem(nodes.First.Value.transform);
		}
	}

	void Update () {
		foreach (GameObject obj in nodes) {
			obj.transform.Translate(Vector3.right * speed * Time.deltaTime);
			if(obj == nodes.First.Value) {
				if(obj.transform.position.x > beltEnd) {
					nodes.AddLast(NewNode(new Vector3(beltStart, 0.0f, -0.5f)));
					SpawnItem(nodes.Last.Value.transform);
					nodes.RemoveFirst();
					Destroy(obj);
					return;
				}
			}
		}
	}

	void SpawnItem(Transform parent) {
		int rand = Random.Range (0, objs.Length);
		GameObject t_obj = (GameObject)Instantiate (objs [rand], parent.position, parent.rotation);
		t_obj.transform.parent = parent;
	}

	GameObject NewNode(Vector3 pos) {
		GameObject t_obj = (GameObject)Instantiate (nodePrefab);
		t_obj.transform.parent = transform;
		t_obj.transform.localPosition = pos;
		return t_obj;
	}
}
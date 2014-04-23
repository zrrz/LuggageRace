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
		//Initialize the whole list. So that they keep their positions, and lack therof, as it loops around. Will add to as you move around levels.
		//Randomize the list each play
		for (int i = 0; i < NumNodes; i++) {
			nodes.AddFirst (NewNode(new Vector3(beltStart + (nodeSize * i), 0.0f, -0.5f)));
		}
		for (LinkedListNode<GameObject> n = nodes.First; n != null; n = n.Next) {
			if(n.Value.GetComponent<Node>().obj == null) {
				SpawnItem(n);
			}
		}
	}

	void Update () {
		foreach (GameObject obj in nodes) {
			obj.transform.localPosition += Vector3.right * speed * Time.deltaTime;
			if(obj == nodes.First.Value) {
				if(obj.transform.localPosition.x > beltEnd) {
					nodes.AddLast(NewNode(new Vector3(beltStart, 0.0f, -0.5f)));
					SpawnItem(nodes.Last.Value.transform);
					nodes.RemoveFirst();
					Destroy(obj);
					return;
				}
			}
		}
	}

	public void PushRight(GameObject node) {
		LinkedListNode<GameObject> curNode = nodes.Find (node);
		Transform lastItem = null;

		while (curNode.Next.Value.transform.childCount != 0 && curNode != null) {
			print ("next");
			Transform curItem = curNode.Value.transform.GetChild(0);
			if(lastItem != null) {
				lastItem.parent = curNode.Value.transform;
				lastItem.localPosition = Vector3.zero;
			}
			lastItem = curItem;
			curNode = curNode.Previous;
		}
	}

	void SpawnItem(Node parent) {
		int rand = Random.Range (0, objs.Length);
		GameObject t_obj = (GameObject)Instantiate (objs [rand], parent.transform.position, parent.transform.rotation);
		t_obj.transform.parent = parent;

		parent.obj = t_obj;

		Item t_item = t_obj.GetComponent<Item>();
		for (int i = 0, n = t_item.filled[0].Length; i < n; i++) {

		}
	}

	GameObject NewNode(Vector3 pos) {
		GameObject t_obj = (GameObject)Instantiate (nodePrefab);
		t_obj.transform.parent = transform;
		t_obj.transform.localPosition = pos;
		return t_obj;
	}
}
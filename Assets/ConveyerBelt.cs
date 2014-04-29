using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyerBelt : MonoBehaviour {

	public List<BeltNode> nodes;

	public float speed = 1.0f;

	public GameObject[] objs;

	public GameObject nodePrefab;

	float beltStart;
	float beltEnd;
	float nodeSize;

	public int beltLength = 10;
	public int numNodes = 30;

	int nodeIndex = 0;

	void Start() {
		beltStart = transform.position.x - transform.FindChild("Belt").localScale.x / 2.0f;
		//nodeSize = transform.FindChild ("Belt").localScale.x / beltLength;
		nodeSize = nodePrefab.transform.localScale.x;

		nodes = new List<BeltNode> ();
		for (int i = 0; i < numNodes; i++) {	
			if(i > beltLength) 
				nodes.Add(NewNode(Vector3.one*1000.0f));
			else
				nodes.Add(NewNode(new Vector3(beltStart + (nodeSize * i), 0.0f, -0.5f)));
		}
		for (int i = 0; i < numNodes; i++) {
			if(nodes[i].obj == null)
				SpawnItem(i);
		}
		for(int i = 0; i < beltLength + 1; i++) {
			nodes[i].on = true;
		}
		nodeIndex = 0;

	}

	void Update() {
		if(nodes[(nodeIndex + beltLength) % numNodes].transform.position.x > transform.FindChild("Belt").localScale.x/2.0f) {
			nodes[(nodeIndex + beltLength) % numNodes].on = false;
			nodes[(nodeIndex + beltLength) % numNodes].transform.localPosition = new Vector3(1000f, 0f, -0.5f);
			nodeIndex--;
			if(nodeIndex < 0) nodeIndex = numNodes - 1;
			nodes[nodeIndex].on = true;
			//nodes[nodeIndex].transform.localPosition = new Vector3(beltStart, 0f, -0.5f);
			nodes[nodeIndex].transform.localPosition = new Vector3((nodes[(nodeIndex+1) % numNodes].transform.position.x) - nodeSize, 0f, -0.5f);
		}
	}

	void SpawnItem(int index) {
		int rand = Random.Range (0, objs.Length);

		Item t_item = ((GameObject)Instantiate (objs[rand])).GetComponent<Item>();

		if (t_item.width + index > numNodes) {
			Destroy(t_item.gameObject);
			return;
		}

		t_item.transform.parent = nodes[index].transform;

		//float buffer = nodeSize - nodePrefab.transform.localScale.x;

		t_item.transform.localPosition = new Vector3((t_item.width - 1)/2.0f, 1 - 1.0f/t_item.height, 0.0f);
		//Vector3 pos = new Vector3 (0f, (1 - 1.0f/t_item.height), -0.5f); 
		//if(t_item.width % 2 == 0) //even
		//	pos.x += (nodes[index].transform.position.x + nodes[(index + 1) % numNodes].transform.position.x)/2.0f;
		//else
		//	pos.x = nodes[(index + 1) % numNodes].transform.position.x;
			
		//t_item.transform.localPosition = pos;

		for (int i = 0; i < t_item.width; i++) {
			nodes[i + index].obj = t_item.gameObject;
		}
	}

	BeltNode NewNode(Vector3 pos) {
		BeltNode t_obj = ((GameObject)Instantiate (nodePrefab)).GetComponent<BeltNode>();
		t_obj.transform.parent = transform;
		t_obj.transform.localPosition = pos;
		t_obj.speed = speed;
		return t_obj;
	}
}
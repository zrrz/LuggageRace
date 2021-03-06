﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyerBelt : MonoBehaviour {

	[System.NonSerialized]
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

	//ItemGrid itemGrid;

	void Start() {
	//	itemGrid = ObjectManager.instance.itemGrid;

		beltStart = transform.position.x - transform.FindChild("Belt").localScale.x / 2.0f;
		nodeSize = nodePrefab.transform.localScale.x;

		nodes = new List<BeltNode> ();
		for (int i = 0; i < numNodes; i++) {	
			if(i > beltLength) 
				nodes.Add(NewNode(Vector3.one*1000.0f));
			else
				nodes.Add(NewNode(new Vector3(beltStart + (nodeSize * i), 0.0f, -0.5f)));
		}
		for (int i = 0; i < numNodes; i++) {
			nodes[i].index = i;
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

		//t_item.transform.localScale = new Vector3 (itemGrid.nodeWidth * t_item.width, itemGrid.nodeHeight * t_item.height, 1.0f);

		t_item.transform.parent = nodes[index].transform;

		t_item.transform.localPosition = new Vector3((t_item.width - 1)/2.0f, 1 - 1.0f/t_item.height, -0.5f);

		//t_item.transform.FindChild ("TopLeft").localPosition = new Vector3 (-t_item.width / 4.0f, t_item.height / 4.0f, 0.0f);

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
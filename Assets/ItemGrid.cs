using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemGrid : MonoBehaviour {

	public GameObject nodePrefab;

	public int rows = 5, columns = 5;

	float gridStartX;
	float gridStartY;
	float nodeSizeX;
	float nodeSizeY;

	public GameObject[,] nodes;

	[System.NonSerialized]
	public float nodeWidth, nodeHeight;

	void Start () {
		//nodes = new List<GameObject>();
		nodes = new GameObject[columns, rows];

		Transform back = transform.FindChild ("Back");

		float gridWidth = back.localScale.x;
		//float nodeWidth = nodePrefab.transform.localScale.x;
		nodeWidth = gridWidth / columns;
		//float gridWidth = nodeWidth*(columns - 1) ;

		float gridHeight = back.localScale.y;
		//float nodeHeight = nodePrefab.transform.localScale.y;
		nodeHeight = gridHeight / rows;
		//float gridHeight = nodeHeight*(rows - 1);

		//float border = nodeWidth/2.0f;
		//float w4 = ((gridWidth - 2 * border) - nodeWidth) / (columns - 1);
		//float h4 = ((gridHeight - 2 * border) - nodeHeight) / (rows - 1);

		gridStartX = back.position.x - gridWidth/2.0f + nodeWidth/2.0f;
		gridStartY = back.position.y - gridHeight/2.0f + nodeHeight/2.0f;

		for (int i = 0; i < columns; i++) {
			for(int j = 0; j < rows; j++) {
				GameObject t_obj = (GameObject)Instantiate(
					nodePrefab, 
					//new Vector3(gridStartX + (w4*i), gridStartY + (h4*j), -0.5f), 
					new Vector3(gridStartX + (nodeWidth*i), gridStartY + (nodeHeight*j), -0.5f),
					Quaternion.identity
				);
				t_obj.transform.parent = transform;
				t_obj.name += i + " " + j;
				t_obj.transform.localScale = new Vector3(nodeWidth, nodeHeight, 1.0f);
				nodes[i,j] = t_obj;
			}
		}
		for (int i = 0; i < columns; i++) {
			for(int j = 0; j < rows; j++) {
				Node t_node = nodes[i,j].GetComponent<Node>();
				t_node.xPos = i;
				t_node.yPos = j;
			}
		}
	}
}

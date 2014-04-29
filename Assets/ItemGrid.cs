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

	//public List<GameObject> nodes;

	public GameObject[,] nodes;

	void Start () {
		//nodes = new List<GameObject>();
		nodes = new GameObject[columns, rows];

		float gridWidth = transform.FindChild ("Back").localScale.x;
		float nodeWidth = nodePrefab.transform.localScale.x;
		float gridHeight = transform.FindChild ("Back").localScale.y;
		float nodeHeight = nodePrefab.transform.localScale.y;

		float border = nodeWidth/2.0f;
		float w4 = ((gridWidth - 2 * border) - nodeWidth) / (columns - 1);
		float h4 = ((gridHeight - 2 * border) - nodeHeight) / (rows - 1);

		gridStartX = transform.position.x - gridWidth/2.0f + border*2;
		gridStartY = transform.position.y - gridHeight/2.0f + border*2;

		for (int i = 0; i < columns; i++) {
			for(int j = 0; j < rows; j++) {
				GameObject t_obj = (GameObject)Instantiate(
					nodePrefab, 
					new Vector3(gridStartX + (w4*i), gridStartY + (h4*j), -0.5f), 
					Quaternion.identity
				);
				t_obj.transform.parent = transform;
				t_obj.name += i + " " + j;
				nodes[i,j] = t_obj;
			}
		}
		for (int i = 0; i < columns; i++) {
			for(int j = 0; j < rows; j++) {
				Node t_node = nodes[i,j].GetComponent<Node>();
				t_node.xPos = i;
				t_node.yPos = j;
				if(i > 0) {
					t_node.left = nodes[i-1, j].GetComponent<Node>();
				}
				if(i < columns - 1) {
					t_node.right = nodes[i+1, j].GetComponent<Node>();
				}
				if(j > 0) {
					t_node.down = nodes[i, j-1].GetComponent<Node>();
				}
				if(j < rows - 1) {
					t_node.up = nodes[i, j+1].GetComponent<Node>();
				}
			}
		}
	}
}

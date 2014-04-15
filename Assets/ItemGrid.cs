using UnityEngine;
using System.Collections;

public class ItemGrid : MonoBehaviour {

	public GameObject nodePrefab;

	public int rows = 5, columns = 5;

	float gridStartX;
	float gridStartY;
	float nodeSizeX;
	float nodeSizeY;

	void Start () {
		float gridWidth = transform.FindChild ("Back").localScale.x;
		float nodeWidth = nodePrefab.transform.localScale.x;
		float gridHeight = transform.FindChild ("Back").localScale.y;
		float nodeHeight = nodePrefab.transform.localScale.y;

		gridStartX = transform.position.x - gridWidth/2.0f + nodeWidth/2.0f;
		gridStartY = transform.position.y + gridHeight/2.0f;
		nodeSizeX = (gridWidth - nodeWidth) / (columns - 1);
		nodeSizeY = (gridHeight) / (rows - 1);

		for (int i = 0; i < columns; i++) {
			for(int j = 0; j < rows; j++) {
				GameObject t_obj = (GameObject)Instantiate(
					nodePrefab, 
					new Vector3(gridStartX + (nodeSizeX*i), gridStartY - (nodeSizeY*j), -0.5f), 
					Quaternion.identity
				);
				t_obj.transform.parent = transform;
			}
		}
	}
}

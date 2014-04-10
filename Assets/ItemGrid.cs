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
		gridStartX = transform.position.x - transform.FindChild("Back").localScale.x / 2.0f;
		gridStartY = transform.position.y - transform.FindChild("Back").localScale.y / 2.0f;
		nodeSizeX = transform.FindChild ("Back").localScale.x / rows;
		nodeSizeY = transform.FindChild ("Back").localScale.y / columns;

		for (int i = 0; i < rows; i++) {
			for(int j = 0; j < columns; j++) {
				GameObject t_obj = (GameObject)Instantiate(
					nodePrefab, 
					new Vector3(gridStartX + (nodeSizeX*i), gridStartY + (nodeSizeY*j), -0.5f), 
					Quaternion.identity
				);
			}
		}
	}

	void Update () {
	
	}
}

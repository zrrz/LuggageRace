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

	public Node[,] nodes;

	[System.NonSerialized]
	public float nodeWidth, nodeHeight;

	void Start () {
		//nodes = new List<GameObject>();
		nodes = new Node[columns, rows];

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
				nodes[i,j] = t_obj.GetComponent<Node>();
			}
		}
		for (int i = 0; i < columns; i++) {
			for(int j = 0; j < rows; j++) {
				nodes[i,j].xPos = i;
				nodes[i,j].yPos = j;
			}
		}
	}

	void Update() {
		List<GameObject> objsChecked = new List<GameObject> ();
		for(int y = 1; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				if(nodes[x,y].obj != null) {
					if(objsChecked.Contains(nodes[x,y].obj) == false) {
						Item t_obj = nodes[x,y].obj.GetComponent<Item>();
						//if(y - t_obj.height > -1) {
							objsChecked.Add(t_obj.gameObject);
							//x -= t_obj.width - 1;

							bool fall = true;
							for(int i = 0; i < t_obj.width; i++) {
								if(nodes[x + i, y - 1].obj != null)
									fall = false;
							}
							if(fall) {
								print ("removing: " + x + " " + y + " placing: " + x + " " + (y - 1));
								RemoveItem(nodes[x,y]);
								PlaceItem(nodes[x, y - 1], t_obj.gameObject);
							}
						//}
					}
				}
			}
		}
	}

	void RemoveItem(Node node) {
		Item item = node.obj.GetComponent<Item>();
		
		int x = node.xPos, y = node.yPos;
		
		int width = item.width;
		int height = item.height;
			
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				if(item.filled[i,j]) {
					nodes[x,y].obj = null;
				}
				x += item.dirY.x;
				y += item.dirY.y;
			}
			x -= item.dirY.x * height;
			y -= item.dirY.y * height;
			
			x += item.dirX.x;
			y += item.dirX.y;
		}
	}

	void PlaceItem(Node node, GameObject obj) {		
		obj.transform.parent = node.transform;
		obj.transform.localPosition = obj.transform.position - obj.transform.FindChild("TopLeft").position + Vector3.back;


		if(obj.tag == "Bag") {
			Bag bag = obj.GetComponent<Bag>();
			bag.topLeftX = node.xPos;
			bag.topLeftY = node.yPos;
		}
		
		Item item = obj.GetComponent<Item>();
		
		int x = node.xPos, y = node.yPos;
		
		int width = item.width;
		int height = item.height;	
		
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				if(item.filled[i,j]) {
					nodes[x,y].obj = obj;
				}
				x += item.dirY.x;
				y += item.dirY.y;
			}
			x -= item.dirY.x * height;
			y -= item.dirY.y * height;
			
			x += item.dirX.x;
			y += item.dirX.y;
		}
	}
}
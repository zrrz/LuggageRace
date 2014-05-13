using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabItem : MonoBehaviour {

	Transform grabber;

	public LayerMask posMask;
	public LayerMask nodeMask;
	public LayerMask beltMask;
	public LayerMask itemMask;

	public bool holding = false;
	public bool floating = false;

	public ConveyerBelt conveyerBelt;
	public ItemGrid itemGrid;

	GameObject heldObj;

	List<GameObject> outlined;

	void Start () {
		outlined = new List<GameObject> ();
		conveyerBelt = GameObject.Find ("Conveyer Belt").GetComponent<ConveyerBelt>();
		grabber = transform.FindChild ("Grabber");
		itemGrid = GameObject.Find("ItemGrid").GetComponent<ItemGrid>();
	}

	void Update () {
		UpdateGrabberPos ();

		if(holding) {
			ClearSpot();
			DrawPlacementOutline();

			if(floating) {
				if (Input.GetButtonDown ("Fire1")) { //Pick an item back up
					PickBackUpItem();
				}
			} else {
				if(Input.GetButtonUp("Fire1")) { //Drop an item into node
					DropItem();
				}
				if (Input.GetButtonDown ("Fire2")) {
					heldObj.GetComponent<Item>().Rotate();
				}
			}
		} else {
			if(!floating) {
				if (Input.GetButtonDown ("Fire1")) {
					PickUpItem();
				}
			}
		}
	}

	void PickBackUpItem() {
		RaycastHit hit2;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, 20.0f, itemMask)) {
			if(hit2.collider.gameObject == heldObj) {
				hit2.collider.gameObject.transform.parent = grabber;
				floating = false;
			} else {
				print ("You're already holding an item!");
			}
		}
	}

	void DrawPlacementOutline() {
		RaycastHit hit;
		if(Physics.Raycast(heldObj.transform.FindChild("TopLeft").position - transform.forward, transform.forward, out hit, 20.0f, nodeMask)) {
			GameObject t_obj = hit.collider.gameObject;
			if(t_obj.tag == "Node") {
				Node t_node = t_obj.GetComponent<Node>();
				if(CheckSpot(heldObj, t_obj)) {
					DrawSpot(heldObj, t_obj, Color.blue);
				} else {
					DrawSpot(heldObj, t_obj, Color.red);
				}
			}
		}
	}

	void DropItem() {
		RaycastHit hit;
		Debug.DrawRay(heldObj.transform.FindChild("TopLeft").position, transform.forward, Color.red, 2.0f);
		if(Physics.Raycast(heldObj.transform.FindChild("TopLeft").position - transform.forward, transform.forward, out hit, 20.0f, nodeMask)) {
			GameObject t_obj = hit.collider.gameObject;
			if(t_obj.tag == "Node") {
				Node t_node = t_obj.GetComponent<Node>();
				if(CheckSpot(heldObj, t_obj)) {
					heldObj.transform.parent = t_obj.transform;
					heldObj.transform.localPosition = heldObj.transform.position - heldObj.transform.FindChild("TopLeft").position;
					PlaceItem(heldObj, t_obj);
					heldObj = null;
					holding = false;
				}
			}
		} else if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 20.0f, beltMask)) {
			GameObject t_obj = hit.collider.gameObject;
			if(t_obj.tag == "BeltNode") {
				print ("hit");
				BeltNode t_node = t_obj.GetComponent<BeltNode>();
				if(t_obj.GetComponent<BeltNode>().obj == null) {
					t_node.obj = heldObj;
					
					heldObj.transform.parent = t_obj.transform;
					heldObj.transform.localPosition = Vector3.zero;
					heldObj = null;
					holding = false;
				}
			}
		} else { //Drop an item. Will float there
			FloatItem();
		}
	}

	void FloatItem() {
		heldObj.transform.parent = null;
		floating = true;
	}

	void PickUpItem() {
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 20.0f, itemMask)) {
			GameObject t_obj = hit.collider.gameObject;
			if(t_obj.tag == "Item") {
				//Node t_node = t_obj.transform.parent.GetComponent<Node>();
				//if(t_node)
				RemoveItem(t_obj, t_obj.transform.parent.gameObject);
				//Node t_beltNode = t_obj.transform.parent.GetComponent<BeltNode>();
				//if(t_node)

				t_obj.transform.parent = grabber;
				holding = true;
				heldObj = t_obj;
			}
		}
	}

	void UpdateGrabberPos() {
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 20.0f, posMask)) {
			grabber.transform.position = hit.point;
		}
	}

	void ClearSpot() {
		foreach (GameObject outline in outlined) {
			outline.renderer.material.color = Color.white;
		}
		outlined.Clear ();
	}

	void DrawSpot(GameObject obj, GameObject nodeObj, Color color) {

		Item item = obj.GetComponent<Item>();
		Node node = nodeObj.GetComponent<Node>();
		
		int x = node.xPos, y = node.yPos; //could be issue
		
		int width = obj.GetComponent<Item>().width;
		int height = obj.GetComponent<Item>().height;
		
		
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				if(item.filled[i,j]) {
					if(x > -1 && x < itemGrid.columns) {
						if(y > -1 && y < itemGrid.rows) {
							for(int k = 0; k < transform.childCount; k++) {
								if(itemGrid.nodes[x,y].transform.GetChild(k).tag == "Node") {
									itemGrid.nodes[x,y].transform.GetChild(k).renderer.material.color = color;
									outlined.Add(itemGrid.nodes[x,y].transform.GetChild(k).gameObject);
								}
							}
						}
					}
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

	bool CheckSpot(GameObject obj, GameObject nodeObj) {
		
		Item item = obj.GetComponent<Item>();
		Node node = nodeObj.GetComponent<Node>();
		
		int x = node.xPos, y = node.yPos; //could be issue
		
		int width = obj.GetComponent<Item>().width;
		int height = obj.GetComponent<Item>().height;
		
		
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				if(item.filled[i,j]) {
					if(x < 0 || x > itemGrid.columns - 1)
						return false;
					if(y < 0 || y > itemGrid.rows - 1)
						return false;
					if(itemGrid.nodes[x,y].GetComponent<Node>().obj != null)
						return false;
				}
				x += item.dirY.x;
				y += item.dirY.y;
			}
			x -= item.dirY.x * height;
			y -= item.dirY.y * height;
			
			x += item.dirX.x;
			y += item.dirX.y;
		}
		return true;
	}

	void PlaceItem(GameObject obj, GameObject nodeObj) {

		Item item = obj.GetComponent<Item>();
		Node node = nodeObj.GetComponent<Node>();

		int x = node.xPos, y = node.yPos; //could be issue

		int width = obj.GetComponent<Item>().width;
		int height = obj.GetComponent<Item>().height;


		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				if(item.filled[i,j]) {
					itemGrid.nodes[x,y].GetComponent<Node>().obj = obj;
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

	void RemoveItem(GameObject obj, GameObject nodeObj) {

		Node t_node = nodeObj.GetComponent<Node>();
		if(t_node) {
			Item item = obj.GetComponent<Item>();
			Node node = nodeObj.GetComponent<Node>();
			
			int x = node.xPos, y = node.yPos; //could be issue
			
			int width = obj.GetComponent<Item>().width;
			int height = obj.GetComponent<Item>().height;
			
			
			for(int i = 0; i < width; i++) {
				for(int j = 0; j < height; j++) {
					if(item.filled[i,j]) {
						itemGrid.nodes[x,y].GetComponent<Node>().obj = null;
					}
					x += item.dirY.x;
					y += item.dirY.y;
				}
				x -= item.dirY.x * height;
				y -= item.dirY.y * height;
				
				x += item.dirX.x;
				y += item.dirX.y;
			}
		} else {
			BeltNode t_beltNode = nodeObj.GetComponent<BeltNode>();
			if(t_beltNode) {
				Item t_item = obj.GetComponent<Item>();
				int index = t_beltNode.index;
				for(int i = 0; i < t_item.width; i++) {
					//NOTE: Mod it to wrap around
					conveyerBelt.nodes[(index + i) % conveyerBelt.numNodes].obj = null;
				}
			}
		}
	}
}
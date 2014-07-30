using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabItem : MonoBehaviour {

	Transform grabber;

	public LayerMask posMask;
	public LayerMask nodeMask;
	public LayerMask beltMask;
	public LayerMask itemMask;
	public LayerMask bagMask;

	public bool holding = false;
	public bool floating = false;

	ConveyerBelt conveyerBelt;
	ItemGrid itemGrid;

	GameObject heldObj;

	List<GameObject> outlined;

	public static GrabItem s_instace;

	[System.NonSerialized]
	public Vector3 mousePos;

	public bool bagOpen = false;

	void Start () {
		s_instace = this;
		outlined = new List<GameObject> ();
		conveyerBelt = ObjectManager.instance.conveyerBelt;
		itemGrid = ObjectManager.instance.itemGrid;

		grabber = transform.FindChild ("Grabber");
	}

	void Update () {
		UpdateGrabberPos ();

		if(GameManager.instance.gameRunning) {
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
					if (Input.GetButtonDown("Fire2")) {
						ToggleBag();
					}
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
				if(CheckSpot(heldObj, t_obj)) {
					DrawSpot(heldObj, t_obj, new Color(0f, 0f, 1f, 0.5f));
				} else {
					DrawSpot(heldObj, t_obj, new Color(1f, 0f, 0f, 0.5f));
				}
			}
		}
	}

	void DropItem() {
		RaycastHit hit;
		if(Physics.Raycast(heldObj.transform.FindChild("TopLeft").position - transform.forward, transform.forward, out hit, 20.0f, nodeMask)) {
			GameObject t_obj = hit.collider.gameObject;
			if(t_obj.tag == "Node") {
				if(CheckSpot(heldObj, t_obj)) {
					PlaceItem(heldObj, t_obj);
					heldObj.transform.parent = t_obj.transform;
					heldObj.transform.localPosition = heldObj.transform.position - heldObj.transform.FindChild("TopLeft").position + Vector3.back;

					holding = false;
					ClearSpot();

					List<SpriteRenderer> renderers = new List<SpriteRenderer> (heldObj.GetComponentsInChildren<SpriteRenderer>());
					if(heldObj.GetComponent<SpriteRenderer>() != null)
						renderers.Add(heldObj.GetComponent<SpriteRenderer>());
					
					foreach(SpriteRenderer rend in renderers) {
						rend.sortingOrder--;
					}

					heldObj = null;
				}
			}
		} else if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 20.0f, beltMask)) {
			GameObject t_obj = hit.collider.gameObject;
			if(t_obj.tag == "BeltNode") {
				if(CheckSpot(heldObj, t_obj)) {
					Item t_item = heldObj.GetComponent<Item>();
					t_item.ResetRotation();
					PlaceItem(heldObj, t_obj);
					heldObj.transform.parent = t_obj.transform;
					heldObj.transform.localPosition = new Vector3((t_item.width - 1)/2.0f, 1 - 1.0f/t_item.height, 0.0f) + Vector3.back;
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

	void ToggleBag() {
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 20.0f, itemMask)) {
			GameObject t_obj = hit.collider.gameObject;
			if(t_obj.tag == "Bag") {
				if(bagOpen) {
					if(t_obj.GetComponent<Bag>().open) {
						t_obj.GetComponent<Bag>().ToggleBag();
						bagOpen = false;
					}
				} else {
					t_obj.GetComponent<Bag>().ToggleBag();
					bagOpen = true;
				}
			}
		}
	}

	void PickUpItem() {
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 20.0f, itemMask)) {
			GameObject t_obj = hit.collider.gameObject;
			if(t_obj.tag == "Item" || t_obj.tag == "Bag") {
				RemoveItem(t_obj, t_obj.transform.parent.gameObject);

				t_obj.transform.parent = grabber;
				Vector3 pos = t_obj.transform.localPosition;
				pos.z = 0.0f;
				t_obj.transform.localPosition = pos;
				holding = true;
				heldObj = t_obj;

				List<SpriteRenderer> renderers = new List<SpriteRenderer> (heldObj.GetComponentsInChildren<SpriteRenderer>());
				if(heldObj.GetComponent<SpriteRenderer>() != null)
					renderers.Add(heldObj.GetComponent<SpriteRenderer>());

				foreach(SpriteRenderer rend in renderers) {
					rend.sortingOrder++;
				}
			}
		}
	}

	void UpdateGrabberPos() {
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 20.0f, posMask)) {
			grabber.transform.position = hit.point;
			mousePos = hit.point;
		}
	}

	void ClearSpot() {
		foreach (GameObject outline in outlined) {
			outline.renderer.material.color = Color.clear;
		}
		outlined.Clear ();
	}

	void DrawSpot(GameObject obj, GameObject nodeObj, Color color) {

		Item item = obj.GetComponent<Item>();
		Node node = nodeObj.GetComponent<Node>();
		
		int x = node.xPos, y = node.yPos;
		
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
		Node t_node = nodeObj.GetComponent<Node>();
		if(t_node) {
			Item item = obj.GetComponent<Item>();
			
			int x = t_node.xPos, y = t_node.yPos;
			
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
		} else {
			BeltNode t_beltNode = nodeObj.GetComponent<BeltNode>();
			if(t_beltNode) {
				Item t_item = obj.GetComponent<Item>();
				int index = t_beltNode.index;
				for(int i = 0; i < t_item.width; i++) {
					if(conveyerBelt.nodes[(index + i) % conveyerBelt.numNodes].obj != null)
						return false;
				}
			}
			return true;
		}
	}

	void PlaceItem(GameObject obj, GameObject nodeObj) {
		Node t_node = nodeObj.GetComponent<Node>();
		if(t_node) {

			if(obj.tag == "Bag") {
				Bag bag = obj.GetComponent<Bag>();
				bag.topLeftX = t_node.xPos;
				bag.topLeftY = t_node.yPos;
			}

			Item item = obj.GetComponent<Item>();

			int x = t_node.xPos, y = t_node.yPos;

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
		} else {
			BeltNode t_beltNode = nodeObj.GetComponent<BeltNode>();
			if(t_beltNode) {
				Item t_item = obj.GetComponent<Item>();
				int index = t_beltNode.index;
				for(int i = 0; i < t_item.width; i++) {
					conveyerBelt.nodes[(index + i) % conveyerBelt.numNodes].obj = obj;
				}
			}
		}
	}

	void RemoveItem(GameObject obj, GameObject nodeObj) {

		Node t_node = nodeObj.GetComponent<Node>();
		if(t_node) {
			Item item = obj.GetComponent<Item>();
			
			int x = t_node.xPos, y = t_node.yPos;
			
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
					conveyerBelt.nodes[(index + i) % conveyerBelt.numNodes].obj = null;
				}
			}
		}
	}
}
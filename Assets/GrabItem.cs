using UnityEngine;
using System.Collections;

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

	void Start () {
		conveyerBelt = GameObject.Find ("Conveyer Belt").GetComponent<ConveyerBelt>();
		grabber = transform.FindChild ("Grabber");
		itemGrid = GameObject.Find("ItemGrid").GetComponent<ItemGrid>();
	}

	void Update () {
		UpdateGrabberPos ();

		if(holding) {
			if (Input.GetButtonDown ("Fire2")) {
				heldObj.GetComponent<Item>().Rotate();
			}
		}

		if(!holding) {
			if(!floating) {
				if (Input.GetButtonDown ("Fire1")) {
					PickUpItem();
				}
			}
		} else {
			if(floating) {
				PickBackUpItem();
			} else {
				if(Input.GetButtonUp("Fire1")) { //Drop an item into node
					DropItem();
				}
			}
		}
	}

	void PickBackUpItem() {
		if (Input.GetButtonDown ("Fire1")) { //Pick an item back up
			RaycastHit hit2;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, 20.0f, itemMask)) {
				if(hit2.collider.gameObject == heldObj) {
					hit2.collider.gameObject.transform.parent = grabber;
					grabber.GetChild(0).localPosition = Vector3.zero;
					floating = false;
				} else {
					print ("You're already holding an item!");
				}
			}
		}
	}

	void DropItem() {
		RaycastHit hit2;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, 20.0f, nodeMask)) {
			GameObject t_obj = hit2.collider.gameObject;
			if(t_obj.tag == "Node") {
				Node t_node = t_obj.GetComponent<Node>();
				if(t_obj.GetComponent<Node>().obj == null) {
					//t_node.obj = grabber.GetChild(0).gameObject;
					
					grabber.GetChild(0).parent = t_obj.transform;
					t_obj.transform.GetChild(0).localPosition = Vector3.zero;
					PlaceItem(t_obj.transform.GetChild(0).gameObject, t_obj);
				}
				holding = false;
			}
		} else if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, 20.0f, beltMask)) {
			GameObject t_obj = hit2.collider.gameObject;
			if(t_obj.tag == "Node") {
				BeltNode t_node = t_obj.GetComponent<BeltNode>();
				if(t_obj.GetComponent<BeltNode>().obj == null) {
					t_node.obj = grabber.GetChild(0).gameObject;
					
					grabber.GetChild(0).parent = t_obj.transform;
					t_obj.transform.GetChild(0).localPosition = Vector3.zero;
				}
				holding = false;
			}
		} else { //Drop an item. Will float there
			grabber.GetChild(0).parent = null;
			floating = true;
		}
	}

	void PickUpItem() {
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 20.0f, itemMask)) {
			GameObject t_obj = hit.collider.gameObject;
			if(t_obj.tag == "Item") {
				Node t_node = t_obj.transform.parent.GetComponent<Node>();
				if(t_node)
					t_node.obj = null;
				
				t_obj.transform.parent = grabber;
				//t_obj.transform.localPosition = Vector3.zero;
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
			x -= item.dirY.x;
			y -= item.dirY.y;

			x += item.dirX.x;
			y += item.dirX.y;
		}
	}
}

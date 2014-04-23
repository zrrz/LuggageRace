using UnityEngine;
using System.Collections;

public class GrabItem : MonoBehaviour {

	Transform grabber;

	public LayerMask posMask;
	public LayerMask nodeMask;
	public LayerMask itemMask;

	public bool holding = false;
	public bool floating = false;

	public ConveyerBelt conveyerBelt;
	public ItemGrid itemGrid;

	GameObject heldObj;

	void Start () {
		conveyerBelt = GameObject.Find ("Conveyer Belt").GetComponent<ConveyerBelt>();
		grabber = transform.FindChild ("Grabber");
	}

	void Update () {
		RaycastHit hit1;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit1, 20.0f, posMask)) {
			grabber.transform.position = hit1.point;
		}

		if(!holding) {
			if(!floating) {
				if (Input.GetButtonDown ("Fire1")) { //Grab an item
					RaycastHit hit2;
					if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, 20.0f, itemMask)) {
						GameObject t_obj = hit2.collider.gameObject;
						print (t_obj.name);
						if(t_obj.tag == "Item") {
							t_obj.transform.parent = grabber;
							t_obj.transform.localPosition = Vector3.zero;
							holding = true;
							heldObj = t_obj;
						}
					}
				}
			}
		} else {
			if(floating) {
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
			} else {
				if(Input.GetButtonUp("Fire1")) { //Drop an item into node
					RaycastHit hit2;
					if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, 20.0f, nodeMask)) {
						GameObject t_obj = hit2.collider.gameObject;
						if(t_obj.tag == "Node") {
							Node t_node = t_obj.GetComponent<Node>();
							if(t_obj.transform.childCount == 0) {
								t_node.obj = grabber.GetChild(0).gameObject;

								grabber.GetChild(0).parent = t_obj.transform;
								t_obj.transform.GetChild(0).localPosition = Vector3.zero;

							} else {
								conveyerBelt.PushRight(t_obj);
							}
							holding = false;
						}
					} else { //Drop an item. Will float there
						grabber.GetChild(0).parent = null;
						floating = true;
					}
				}
			}
		}
	}

	const int WIDTH = 3;
	const int HEIGHT = 2;

	void PlaceItem(GameObject obj, GameObject nodeObj) {
		Item item = obj.GetComponent<Item>();
		Node node = nodeObj.GetComponent<Node>();

		int x = 0, y = 0;

		//fix to reset at top each width iteration
		while (x < WIDTH) {
			while(y < HEIGHT) {
				if(item.filled[x][y]) {
					itemGrid.nodes[x*HEIGHT+y].obj = obj;
				}
				x += item.dirY.x;
				y += item.dirY.y;
			}
			x += item.dirX.x;
			y += item.dirX.y;
		}
	}
}

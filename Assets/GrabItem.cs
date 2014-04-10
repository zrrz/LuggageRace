using UnityEngine;
using System.Collections;

public class GrabItem : MonoBehaviour {

	Transform grabber;

	public LayerMask posMask;
	public LayerMask nodeMask;

	bool holding = false;

	void Start () {
		grabber = transform.FindChild ("Grabber");
	}

	void Update () {
		RaycastHit hit1;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit1, 20.0f, posMask)) {
			print ("hit");
			grabber.transform.position = hit1.point;
		}

		if(!holding) {
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit hit2;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2)) {
					GameObject t_obj = hit2.collider.gameObject;
					if(t_obj.tag == "Item") {
						t_obj.transform.parent = grabber;
						t_obj.transform.localPosition = Vector3.zero;
						holding = true;
					}
				}
			}
		} else {
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit hit2;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, 20.0f, nodeMask)) {
					GameObject t_obj = hit2.collider.gameObject;
					if(t_obj.tag == "Node") {
						if(t_obj.transform.childCount == 0) {
							grabber.GetChild(0).parent = t_obj.transform;
							t_obj.transform.GetChild(0).localPosition = Vector3.zero;
							holding = false;
						}
					}
				}
			}
		}
	}
}

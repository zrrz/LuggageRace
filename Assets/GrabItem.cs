﻿using UnityEngine;
using System.Collections;

public class GrabItem : MonoBehaviour {

	Transform grabber;

	public LayerMask posMask;
	public LayerMask nodeMask;
	public LayerMask itemMask;

	bool holding = false;

	public ConveyerBelt conveyerBelt;

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
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit hit2;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, 20.0f, itemMask)) {
					GameObject t_obj = hit2.collider.gameObject;
					print (t_obj.name);
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
						} else {
							conveyerBelt.PushRight(t_obj);
						}
						holding = false;
					}
				}
			}
		}
	}
}

﻿using UnityEngine;
using System.Collections;

public class Bag : Item {

	[System.NonSerialized]
	public int topLeftX;

	[System.NonSerialized]
	public int topLeftY;

	ItemGrid itemGrid;

	GameObject grayscalePlane;

	GameObject inside;
	GameObject cover;

	bool open = false;

	void Start () {
		inside = transform.FindChild ("Inside").gameObject;
		cover = transform.FindChild ("Cover").gameObject;

		inside.SetActive (false);

		itemGrid = ObjectManager.instance.itemGrid;
		grayscalePlane = ObjectManager.instance.grayscalePlane;


		filled = new bool[width, height];
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				filled[i,j] = true;
			}
		}
		dirX = rotationTable[rotTableIterX];
		dirY = rotationTable[rotTableIterY];
	}

	public void ToggleBag() {
		if(!open) {
			open = true;

			inside.SetActive (true);
			cover.SetActive (false);
			grayscalePlane.SetActive (true);

			transform.localPosition += Vector3.back * 2.0f;

			int x = topLeftX, y = topLeftY;

			for(int i = 0; i < width; i++) {
				for(int j = 0; j < height; j++) {
					itemGrid.nodes[x,y].GetComponent<Node>().obj = null;
					x += dirY.x;
					y += dirY.y;
				}
				x -= dirY.x * height;
				y -= dirY.y * height;
				
				x += dirX.x;
				y += dirX.y;
			}
		} else {
			open = false;

			inside.SetActive (false);
			cover.SetActive (true);
			grayscalePlane.SetActive (false);
			
			transform.localPosition -= Vector3.back * 2.0f;
			
			int x = topLeftX, y = topLeftY;
			
			for(int i = 0; i < width; i++) {
				for(int j = 0; j < height; j++) {
					itemGrid.nodes[x,y].GetComponent<Node>().obj = gameObject;
					x += dirY.x;
					y += dirY.y;
				}
				x -= dirY.x * height;
				y -= dirY.y * height;
				
				x += dirX.x;
				y += dirX.y;
			}
		}
	}
}
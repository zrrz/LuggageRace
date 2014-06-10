using UnityEngine;
using System.Collections;

public class Luggage : Item {
	
	void Start () {
		filled = new bool[width, height];
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				filled[i,j] = true;
			}
		}
		dirX = rotationTable[rotTableIterX];
		dirY = rotationTable[rotTableIterY];
	}

	void Update () {
	
	}
}
using UnityEngine;
using System.Collections;

public class LBox : Item {

	void Start () {
		//A constant value is expected
		//filled = new bool[width, height] {
		//	{true, true, true},
		//	{false, true, false}
		//};
		
		//Temporary
		filled = new bool[width, height];
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				filled[i,j] = true;
			}
		}
		filled [0, 0] = false;
		//filled [2, 1] = false;
		
		dirX = rotationTable[rotTableIterX];
		dirY = rotationTable[rotTableIterY];

		Init ();
	}

	void Update() {
		MoveEyes ();
	}
}

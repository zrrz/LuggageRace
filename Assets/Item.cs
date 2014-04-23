using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public bool [][] filled;

	public class Dir {
		public Dir(int t_x, int t_y) { x = t_x; y = t_y;}
		public int x, y;
	};

	int rotTableIterX = 0;
	int rotTableIterY = 1;

	static Dir[] rotationTable = {new Dir(1, 0), new Dir(0, -1), new Dir(-1, 0), new Dir(0, 1)}; //Will it only "new" once?

	public Dir dirX;
	public Dir dirY;

	const int ROT_TABLE_SIZE = 4;

	void Start () {
		dirX = rotationTable[rotTableIterX];
		dirY = rotationTable[rotTableIterY];
	}

	void Update () {
	
	}

	public void Rotate() {
		dirX = rotationTable[SafeAdd(rotTableIterX)];
		dirY = rotationTable[SafeAdd(rotTableIterY)];
		transform.Rotate(Vector3.forward * 90.0f);
	}

	int SafeAdd(int iter) {
		iter++;
		if(iter > ROT_TABLE_SIZE - 1) {
			iter = 0;
		}
		return iter;
	}
}
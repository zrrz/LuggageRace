using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public bool [,] filled;

	public class Dir {
		public Dir(int t_x, int t_y) { x = t_x; y = t_y;}
		public int x, y;
	};

	public int width, height;

	protected int rotTableIterX = 0;
	protected int rotTableIterY = 1;
	
	static protected Dir[] rotationTable = {new Dir(1, 0), new Dir(0, -1), new Dir(-1, 0), new Dir(0, 1)};

	public Dir dirX;
	public Dir dirY;

	const int ROT_TABLE_SIZE = 4;

	protected GameObject lEye, rEye;

	Vector3 lEyeStartOffset, rEyeStartOffset;

	public float eyeRange = 0.05f;

	void Start () {
		Init ();
	}

	protected void Init() {
		lEye = transform.FindChild("L_Eye").gameObject;
		rEye = transform.FindChild("R_Eye").gameObject;
		if(lEye)
			lEyeStartOffset = lEye.transform.position - transform.position;
		if(rEye)
			rEyeStartOffset = rEye.transform.position - transform.position;
		
		filled = new bool[width, height];
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				filled[i,j] = true;
			}
		}
		dirX = rotationTable[rotTableIterX];
		dirY = rotationTable[rotTableIterY];
	}

	public void Rotate() {
		rotTableIterX = SafeAdd (rotTableIterX);
		rotTableIterY =	SafeAdd(rotTableIterY);

		dirX = rotationTable[rotTableIterX];
		dirY = rotationTable[rotTableIterY];

		//transform.Rotate(Vector3.forward * 90.0f);
		transform.RotateAround (transform.parent.position, Vector3.forward, -90.0f);
	}

	public void ResetRotation() {
		dirX = rotationTable[0];
		dirY = rotationTable[1];

		transform.eulerAngles = Vector3.zero;
	}

	void Update() {
		MoveEyes ();
	}

	protected void MoveEyes() {
		if(lEye) {
			Vector3 newPos = GrabItem.s_instace.mousePos - (transform.position + lEyeStartOffset);
			lEye.transform.position = transform.position + lEyeStartOffset + (newPos.normalized * eyeRange);
		}
		if(rEye) {
			Vector3 newPos = GrabItem.s_instace.mousePos - (transform.position + rEyeStartOffset);
			rEye.transform.position = transform.position + rEyeStartOffset + (newPos.normalized * eyeRange);
		}
	}

	int SafeAdd(int iter) {
		iter++;
		if(iter > ROT_TABLE_SIZE - 1) {
			iter = 0;
		}
		return iter;
	}
}
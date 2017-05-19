using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour {
	TouchHandler touchHandler;
	float[] last_rotations;
	int circleIndex;
	int layerNum;
	int sectorNum;
	// Use this for initialization
	SectorConfig[] config;
	float[] radiusConfig;
	GameObject[] sectors;
	GameLogic logic;


	void Start () {
		reset ();
		this.touchHandler = new TouchHandler (Camera.main.WorldToScreenPoint(transform.position));
		this.touchHandler.onTouchBegan += onTouchBegin;
		this.touchHandler.onTouchEnd += onTouchEnd;
		this.touchHandler.onTouchMove += onTouchMove;
		this.touchHandler.onTouchStationary += onTouchStationary;

		logic = new GameLogic(GetComponent<Plate>());
	}

	public void reset()
	{
		layerNum = GetComponent<Plate> ().layerNum;
		config = GetComponent<Plate> ().config;
		sectors = GetComponent<Plate> ().sectors;
		sectorNum = GetComponent<Plate> ().seperateNum;
		last_rotations = new float[sectorNum*layerNum];
		radiusConfig = new float[layerNum];
		for (int i = 0; i < layerNum; i++) {
			radiusConfig [i] = config [i * sectorNum].radius;
		}
	}

	// Update is called once per frame
	void Update () {
		if (this.touchHandler != null) {
			this.touchHandler.onUpdate ();
		}
	}

	int getTouchCircleIndex(Vector2 pos) {
		Vector3 touchPos = Camera.main.ScreenToWorldPoint (new Vector3 (pos.x, pos.y, 0));
		Vector3 center = transform.position;
		float distance = Vector2.Distance (new Vector2 (touchPos.x, touchPos.y), new Vector2 (center.x, center.y));
		int ret = layerNum - 1;
		for (int i = 0; i < layerNum; i++) {
//			Debug.Log (distance+" "+radiusConfig [i]);

			if (distance < radiusConfig [i]) {
				ret = i;
				return ret;
			}
		}
		return ret;
	}


	private Quaternion getQuaterionFromAngle(float angle)
	{
		return Quaternion.Euler(new Vector3(0, 0, angle));
	}

	public void onTouchBegin(Vector2 startPos)
	{
//		Debug.Log ("wenkan Main on touch begin");
		circleIndex = getTouchCircleIndex (startPos);
//		Debug.Log ("wenkan " + circleIndex.ToString () + " " + last_rotations.Length + " " +sectors.Length + " " + sectorNum);
		int startIndex = circleIndex * sectorNum;

		for (int i = startIndex; i < startIndex + sectorNum; i++) {
//			Debug.Log (i);
			last_rotations [i] = sectors [i].transform.rotation.eulerAngles.z;
			GetComponent<Plate> ().setSectorRotation (i, last_rotations [i]);
		}

	}

	public void onTouchMove(Vector2 curPos, float angle)
	{
//		Debug.Log ("wenkan "+curPos.ToString()+" "+angle.ToString());

		int startIndex = circleIndex * sectorNum;
		for (int i = startIndex; i < startIndex + sectorNum; i++) {
			float last_rotation = last_rotations [i];
			sectors [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, last_rotation - angle));
			GetComponent<Plate> ().setSectorRotation (i, last_rotation - angle);
		}
		logic.onPlateRotate (circleIndex);
	}

	public void onTouchEnd(Vector2 curPos)
	{
//		Debug.Log ("wenkan Main on touch end");
		int startIndex = circleIndex * sectorNum;
		for (int i = startIndex; i < startIndex + sectorNum; i++) {
			last_rotations [i] = sectors [i].transform.rotation.eulerAngles.z;
			GetComponent<Plate> ().setSectorRotation (i, last_rotations [i]);
		}
		logic.onLeavePlate (circleIndex);


		circleIndex = -1;
	}


	public void onTouchStationary (Vector2 curPos)
	{
		Debug.Log ("wenkan Main on touch stationary");
	}

}

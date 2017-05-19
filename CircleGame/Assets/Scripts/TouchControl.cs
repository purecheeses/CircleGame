using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour {
	TouchHandler touchHandler;
	float[] last_rotations;
	int circleIndex;
	int layerNum;
	// Use this for initialization
	SectorConfig[] config;
	float[] radiusConfig;
	GameObject[] sectors;
	GameLogic logic;


	void Start () {
		layerNum = GetComponent<Plate> ().layerNum;
		config = GetComponent<Plate> ().config;
		sectors = GetComponent<Plate> ().sectors;
		int sectorNum = GetComponent<Plate> ().seperateNum;
		last_rotations = new float[sectorNum*layerNum];
		radiusConfig = new float[layerNum];
		for (int i = 0; i < layerNum; i++) {
			radiusConfig [i] = config [i * sectorNum].radius;
		}
		this.touchHandler = new TouchHandler (Camera.main.WorldToScreenPoint(transform.position));
		this.touchHandler.onTouchBegan += onTouchBegin;
		this.touchHandler.onTouchEnd += onTouchEnd;
		this.touchHandler.onTouchMove += onTouchMove;
		this.touchHandler.onTouchStationary += onTouchStationary;

		logic = new GameLogic(GetComponent<Plate>());
	}
	
	// Update is called once per frame
	void Update () {
		this.touchHandler.onUpdate ();
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
//		Debug.Log ("wenkan " + circleIndex.ToString ());
		int startIndex = circleIndex * layerNum;

		for (int i = startIndex; i < startIndex + layerNum; i++) {
			last_rotations [i] = sectors [i].transform.rotation.eulerAngles.z;
			GetComponent<Plate> ().setSectorRotation (i, last_rotations [i]);
		}

	}

	public void onTouchMove(Vector2 curPos, float angle)
	{
//		Debug.Log ("wenkan "+curPos.ToString()+" "+angle.ToString());

		int startIndex = circleIndex * layerNum;
		for (int i = startIndex; i < startIndex + layerNum; i++) {
			float last_rotation = last_rotations [i];
			sectors [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, last_rotation - angle));
			GetComponent<Plate> ().setSectorRotation (i, last_rotation - angle);
		}
		logic.onPlateRotate (circleIndex);
	}

	public void onTouchEnd(Vector2 curPos)
	{
//		Debug.Log ("wenkan Main on touch end");
		int startIndex = circleIndex * layerNum;
		for (int i = startIndex; i < startIndex + layerNum; i++) {
			last_rotations [i] = sectors [i].transform.rotation.eulerAngles.z;
			GetComponent<Plate> ().setSectorRotation (i, last_rotations [i]);
		}
		circleIndex = -1;
	}

	public void onTouchStationary (Vector2 curPos)
	{
		Debug.Log ("wenkan Main on touch stationary");
	}

}

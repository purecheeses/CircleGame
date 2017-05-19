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
	GameObject[] circles;
	void Start () {
		layerNum = GetComponent<Plate> ().layerNum;
		last_rotations = new float[layerNum];
		config = GetComponent<Plate> ().config;
		circles = GetComponent<Plate> ().circles;
		this.touchHandler = new TouchHandler (Camera.main.WorldToScreenPoint(transform.position));
		this.touchHandler.onTouchBegan += onTouchBegin;
		this.touchHandler.onTouchEnd += onTouchEnd;
		this.touchHandler.onTouchMove += onTouchMove;
		this.touchHandler.onTouchStationary += onTouchStationary;
	}
	
	// Update is called once per frame
	void Update () {
		this.touchHandler.onUpdate ();
	}

	int getTouchCircleIndex(Vector2 pos) {
		Vector3 touchPos = Camera.main.ScreenToWorldPoint (new Vector3 (pos.x, pos.y, 0));
		Vector3 center = transform.position;
		float distance = Vector2.Distance (new Vector2 (touchPos.x, touchPos.y), new Vector2 (center.x, center.y));

		int ret = 3;
		for (int i = 0; i < layerNum; i++) {
			if (distance < config [i].radius) {
				ret = i + 1;
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
		Debug.Log ("wenkan Main on touch begin");
		circleIndex = getTouchCircleIndex (startPos);
		int startIndex = (circleIndex - 1) * layerNum;

		for (int i = startIndex; i < startIndex + layerNum; i++) {
			last_rotations [i] = circles [i].transform.rotation.eulerAngles.z;
		}

	}

	public void onTouchMove(Vector2 curPos, float angle)
	{
		Debug.Log ("wenkan "+curPos.ToString()+" "+angle.ToString());

		int startIndex = (circleIndex - 1) * layerNum;
		for (int i = startIndex; i < startIndex + layerNum; i++) {
			float last_rotation = last_rotations [i];
			circles [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, last_rotation - angle));
		}
	}

	public void onTouchEnd(Vector2 curPos)
	{
		Debug.Log ("wenkan Main on touch end");
		int startIndex = (circleIndex - 1) * layerNum;
		for (int i = startIndex; i < startIndex + layerNum; i++) {
			last_rotations [i] = circles [i].transform.rotation.eulerAngles.z;
		}
		circleIndex = -1;
	}

	public void onTouchStationary (Vector2 curPos)
	{
		Debug.Log ("wenkan Main on touch stationary");
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTool : MonoBehaviour {
	public GameObject plate;
	Vector3 center;
	SectorConfig[] config;
	int layerNum;
	void Start(){
		init ();
	}

	void init(){
		center = plate.transform.position;
		config =plate.GetComponent<Plate> ().config;
		layerNum = plate.GetComponent<Plate> ().layerNum;
	}

	public int getTouchSectorIndex(Vector2 pos) {
		Vector3 touchPos = Camera.main.ScreenToWorldPoint (new Vector3 (pos.x, pos.y, 0));
		Vector3 center = transform.position;
		float distance = Vector2.Distance (new Vector2 (touchPos.x, touchPos.y), new Vector2 (center.x, center.y));
		float touchAngle = Vector2.Angle (touchPos, Vector3.up);
		if (touchPos.x < center.x) {
			touchAngle = 360 - touchAngle;
		}
		for (int i = config.Length - 1; i >= 0; i--) {
			var c = config [i];
			float rot = c.rotation;
			Vector3 vec = Quaternion.AngleAxis (rot, Vector3.forward) * Vector3.up;
			float angle = Vector3.Angle (vec, Vector3.up);
			if (vec.x < center.x) {
				angle = 360 - angle;
			}
			float dist = Vector2.Distance (touchPos, center);
			float diffAngle = Mathf.Abs (angle - touchAngle);
			if (diffAngle >= 180) {
				diffAngle = 360 - diffAngle;
			}
			if (dist <= c.radius && dist >= c.radius- globalConfig.radius/layerNum && diffAngle <= c.angle/2)
				return i;
		}
		return - 1;
	}

	void Update(){
		getMouseClick ();
	}

	void getMouseClick(){
		if (Input.GetMouseButtonDown (0)) {
			Vector3 curPos = Input.mousePosition;
			int index = getTouchSectorIndex (curPos);
			Debug.Log (index);
		}
	}
}

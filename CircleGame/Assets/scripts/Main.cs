using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 3; i++) {
			DrawTool.DrawCircle (transform, transform.position, circleConfig [i]);
		}
		for (int i = 0; i < 9; i++) {
			SectorConfig c = config [i];
			circles [i] = DrawTool.DrawSectorSolid (transform, transform.position, c.angle, c.radius, new Color(c.r,c.g,c.b));
			circles [i].transform.Rotate (new Vector3(0, 0, c.rotation));
			Vector3 t = circles [i].transform.position;
			circles [i].transform.position = new Vector3 (t.x, t.y, c.radius);
		}
	}

	static Color[] pool = { Color.red, Color.blue, Color.green };

	void genNewStruct(int colorCount = 3, int circleCount = 3){
		float[] split = new float[3];

	}

	static GameObject[] circles = new GameObject[9];

	static SectorConfig[] config = {
		new SectorConfig(120f, 0f, 1.0f, 255,0,0),
		new SectorConfig(120f, 120f, 1.0f, 0,255,0),
		new SectorConfig(120f, 240f, 1.0f, 0,0,255),

		new SectorConfig(120f, 25f, 2.0f, 255,2,0),
		new SectorConfig(120f, 145f, 2.0f, 0,255,2),
		new SectorConfig(120f, 265f, 2.0f, 0,0,255),

		new SectorConfig(120f, 45f, 3.0f, 255,2,2),
		new SectorConfig(120f, 165f, 3.0f, 0,255,0),
		new SectorConfig(120f, 285f, 3.0f, 0,0,255),

	};

	static float[] circleConfig = { 1.0f, 2.0f, 3.0f };

	public static GameObject[] getCircles() {
		return circles;
	}

	static int circleIndex = -1;

	// Update is called once per frame
	void Update () {

//		if (Input.GetMouseButtonDown (0))
//			Debug.Log ("wenkan");
//		return;



		if (Input.GetMouseButtonDown (0)) {
//			Debug.Log ("111");
			Vector3 curPos = Input.mousePosition;
			Vector3 center = Camera.main.WorldToScreenPoint (transform.position);
			circleIndex = getTouchCircleIndex (curPos);
		} else if (Input.GetMouseButtonUp (0)) {
//			Debug.Log ("222");
			circleIndex = -1;
		} else if (Input.GetMouseButton (0)) {
//			Debug.Log ("333");
			Vector3 curPos = Input.mousePosition;
			Vector3 center = Camera.main.WorldToScreenPoint (transform.position);
			float angle = getRotation (curPos);

//			Debug.Log (circleIndex);

			for (int i = (circleIndex-1)*3; i < (circleIndex-1)*3 + 3; i++) {

				circles [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, config [i].rotation - angle));
			}
		}

		return;
		if (Input.GetMouseButton (0)) {
			Vector3 curPos = Input.mousePosition;
			Vector3 center = Camera.main.WorldToScreenPoint (transform.position);

			if (circleIndex < 0) {
				circleIndex = getTouchCircleIndex (curPos);
			}

			float angle = getRotation (curPos);

			Debug.Log (circleIndex);

			for (int i = (circleIndex-1)*3; i < (circleIndex-1)*3 + 3; i++) {
				
				circles [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, config [i].rotation - angle));
			}


//			float angle = Vector2.Angle (new Vector2 (0, 1), new Vector2 (curPos.x - center.x, curPos.y - center.y));
			
			return;
		} else {
			circleIndex = -1;
			return;
		}


	}

	int getTouchCircleIndex(Vector2 pos) {
		Vector3 touchPos = Camera.main.ScreenToWorldPoint (new Vector3 (pos.x, pos.y, 0));
		Vector3 center = transform.position;
		float distance = Vector2.Distance (new Vector2 (touchPos.x, touchPos.y), new Vector2 (center.x, center.y));
		if (distance < config[0].radius)
			return 1;
		else if (distance >= config[0].radius && distance < config[3].radius)
			return 2;
		else
			return 3;
	}

	float getRotation(Vector2 mousePos) {
		Vector3 touchPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 0));
		Vector3 center = transform.position;
		int sign = touchPos.x < center.x ? -1 : 1;
		return sign * Vector3.Angle (new Vector3 (0, 1, 0), new Vector3 (touchPos.x - center.x, touchPos.y - center.y, 0));
	}

	void onClick(GameObject obj) {
		obj.transform.Rotate(new Vector3(0, 0, 45));
	}
}

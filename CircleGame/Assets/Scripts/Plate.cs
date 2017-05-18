using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class globalConfig{
	public static float radius = 3f;
}

public struct SectorConfig {
	public SectorConfig(float angle, float rotation, float radius, float r, float g,float b) {
		this.angle = angle;
		this.radius = radius;
		this.r = r; 
		this.g = g;
		this.b = b;
		this.rotation = rotation;
	}
	public float angle;
	public float radius;			
	public float r;
	public float g;
	public float b;
	public float rotation;
}

public class Plate : MonoBehaviour {
	public int layerNum = 3;			//hot many layers in plate
	public int seperateNum = 3;			//how many piece per layer
	[SerializeField]
	public SectorConfig[] config = {
		new SectorConfig(120f, 0f, 1.0f, 255,0,0),
		new SectorConfig(120f, 120f, 1.0f, 0,255,0),
		new SectorConfig(120f, 240f, 1.0f, 0,0,255),

		new SectorConfig(120f, 25f, 2.0f, 255,2,0),
		new SectorConfig(120f, 145f, 2.0f, 0,255,2),
		new SectorConfig(120f, 265f, 2.0f, 0,0,255),

		new SectorConfig(120f, 45f, 3.0f, 255,2,0),
		new SectorConfig(120f, 165f, 3.0f, 0,255,0),
		new SectorConfig(120f, 285f, 3.0f, 0,0,255),
	};
	GameObject[] circles;
	TouchHandler touchHandler;
	// Use this for initialization
	void Start () {

		drawGame ();
		
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

		for (int i = 0; i < layerNum; i++) {

		}

		if (distance < config[0].radius)
			return 1;
		else if (distance >= config[0].radius && distance < config[3].radius)
			return 2;
		else
			return 3;
	}


	private Quaternion getQuaterionFromAngle(float angle)
	{
		return Quaternion.Euler(new Vector3(0, 0, angle));
	}

	public void onTouchBegin(Vector2 startPos)
	{
		Debug.Log ("wenkan Main on touch begin");
		circleIndex = getTouchCircleIndex (startPos);
		int startIndex = (circleIndex - 1) * 3;

		for (int i = startIndex; i < startIndex + 3; i++) {
			last_rotations [i] = circles [i].transform.rotation.eulerAngles.z;
		}

	}

	public void onTouchMove(Vector2 curPos, float angle)
	{
		Debug.Log ("wenkan "+curPos.ToString()+" "+angle.ToString());

		for (int i = (circleIndex-1)*3; i < (circleIndex-1)*3 + 3; i++) {
			float last_rotation = last_rotations [i];
			circles [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, last_rotation - angle));
		}
	}

	public void onTouchEnd(Vector2 curPos)
	{
		Debug.Log ("wenkan Main on touch end");
		int startIndex = (circleIndex - 1) * 3;
		for (int i = startIndex; i < startIndex + 3; i++) {
			last_rotations [i] = circles [i].transform.rotation.eulerAngles.z;
		}
		circleIndex = -1;
	}

	public void onTouchStationary (Vector2 curPos)
	{
		Debug.Log ("wenkan Main on touch stationary");
	}

	void drawFrontier(){
		for (int i = 1; i <= layerNum; i++) {
			GameObject obj = new GameObject ();
			obj.transform.parent = transform;
			DrawTool.DrawCircle (obj.transform, obj.transform.position, globalConfig.radius*i/layerNum);
		}
	}


	void drawSector(){
		circles = new GameObject[layerNum * seperateNum];
		for (int i = 0; i < layerNum * seperateNum; i++) {
			SectorConfig c = config [i];
			circles [i] = DrawTool.DrawSectorSolid (transform, transform.position, c.angle, c.radius, new Color(c.r/255.0f,c.g/255.0f,c.b/255.0f));
			circles [i].transform.Rotate (new Vector3(0, 0, c.rotation));
			Vector3 t = circles [i].transform.position;
			circles [i].transform.position = new Vector3 (t.x, t.y, c.radius);
			circles [i].name = "sector_"+ i.ToString();
		}
	}

	void drawGame(){
		drawFrontier ();
		drawSector ();
	}

	void clean(){
		foreach (Transform t in GetComponentsInChildren<Transform>()) {
			if (transform != t) {
				Destroy (t.gameObject);			
			}
		}
	}

	public void refresh(){
		clean ();
		drawGame ();
	}

	public void makeNewData(){
		config = new SectorConfig[layerNum * seperateNum];
		int n = -1;
		for (int i = 0; i < layerNum; i++) {
			for (int j = 0; j < seperateNum; j++) {
				++n;
				float radius = globalConfig.radius / layerNum * (i + 1);
				float rot = 360 / seperateNum * j;
				float angle = 360 / seperateNum;
				config [n] = new SectorConfig (angle, rot, radius, Random.Range (0, 255), Random.Range (0, 255), Random.Range (0, 255));
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  
using System.Text;
using UnityEngine.UI;

public static class globalConfig{
	public static float radius = 3.0f;
	public static Dictionary<string,string> colorMusicPair = new Dictionary<string, string> (){
		{"102,102,211","do"},
		{"99,186,217","re"},
		{"126,200,110","mi"},
		{"250,246,84","fa"},
		{"249,169,74","so"},
		{"247,87,131","la"},
		{"99,125,233","xi"},
	};

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

		new SectorConfig(120f, 25f, 2.0f, 255,0,0),
		new SectorConfig(120f, 145f, 2.0f, 0,255,0),
		new SectorConfig(120f, 265f, 2.0f, 0,0,255),

		new SectorConfig(120f, 45f, 3.0f, 255,0,0),
		new SectorConfig(120f, 165f, 3.0f, 0,255,0),
		new SectorConfig(120f, 285f, 3.0f, 0,0,255),
	};

	public GameObject[] sectors;
	public float[] sector_rotations; //记录所有扇形当前的旋转角度，后面就不用计算啦

	public void setSectorRotation(int sectorIndex, float rotation)
	{
		sector_rotations [sectorIndex] = rotation;
	}

	public float getSectorRotation(int sectorIndex)
	{
		return sector_rotations [sectorIndex];
	}
		

	// Use this for initialization
	void Awake () {
		drawGame ();

	}

	void Start(){
		open("1-1");
	}
	
	// Update is called once per frame
	void Update () {

	}


	void drawFrontier(){
		for (int i = 1; i <= layerNum; i++) {
			GameObject obj = new GameObject ();
			obj.transform.parent = transform;
			DrawTool.DrawCircle (obj.transform, obj.transform.position, globalConfig.radius*i/layerNum);
		}
	}


	void drawSector(){
		sectors = new GameObject[layerNum * seperateNum];
		for (int i = 0; i < layerNum * seperateNum; i++) {
			SectorConfig c = config [i];
//			Debug.Log ("wenkan dfsdjkfjdsf "+c.angle);
			sectors [i] = DrawTool.DrawSectorSolid (transform, transform.position, c.angle, c.radius, new Color(c.r/255.0f,c.g/255.0f,c.b/255.0f),c.rotation);
			sectors [i].transform.Rotate (new Vector3(0, 0, c.rotation));
			Vector3 t = sectors [i].transform.position;
			sectors [i].transform.position = new Vector3 (t.x, t.y, c.radius);
			sectors [i].name = "sector_"+ i.ToString();
//			Texture tx = Resources.Load ("PaperTexture") as Texture;
//			circles [i].GetComponent<MeshRenderer> ().material .SetTexture ("_MainTex", tx);
		}
	}

	void drawGame(){
		int sectorCount = layerNum * seperateNum;
		sector_rotations = new float[sectorCount];
		for (int i = 0; i < sectorCount; i++) {
			sector_rotations [i] = config [i].rotation;
		}

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
				float rot = 360f / seperateNum * j;
				float angle = 360f / seperateNum;
				config [n] = new SectorConfig (angle, rot, radius, Random.Range (0, 255), Random.Range (0, 255), Random.Range (0, 255));
			}
		}
	}

	public void alignSector(int circleIndex, int sectorIndex)
	{
		float rotation = sector_rotations [sectorIndex];
		rotation = rotation % 360;
		int startIndex = circleIndex * seperateNum;
		for (int i = startIndex; i < startIndex + seperateNum; i++) {
			float last_rotation = sector_rotations [i];
			sectors [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, last_rotation - rotation));
			GetComponent<Plate> ().setSectorRotation (i, last_rotation - rotation);
		}
	}

	public void open(string levelName){
		FileStream file_stream;  
		string file_path = Application.dataPath+"/Datas/"; 
		string file_name = levelName;
		file_stream = File.OpenRead (file_path + "//" + file_name);
		int fsLen = (int)file_stream.Length;
		byte[] heByte = new byte[fsLen];
		int r = file_stream.Read(heByte, 0, heByte.Length);
		string myStr = System.Text.Encoding.UTF8.GetString(heByte);	
		string[] tmpS = myStr.Split ('$');
		layerNum = int.Parse( tmpS [0]);
		seperateNum = int.Parse (tmpS [1]);
		layerNum = layerNum;
		seperateNum = seperateNum;
		config = new SectorConfig[layerNum * seperateNum];
		Debug.Log (layerNum * seperateNum);
		Debug.Log (tmpS.Length);
		for (int i = 2; i < tmpS.Length - 1; i++) {
			string json = tmpS [i];
//			Debug.Log (i - 2);
			config [i - 2] = JsonUtility.FromJson<SectorConfig> (json);
		}
		GetComponent<Plate> ().refresh ();
		//		plate.GetComponent<Plate>().
	}
}

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
	public static List<string> levelList = new List<string> () {
		"1-1", "1-2", "1-3",
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
	public int levelNum = 0;
	public GameObject targetUI;
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
	public string winCond;
	public List<string> winMusic;
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
		if (Camera.main.GetComponent<EditorControl> () == null) {
			open("1-1");
		}
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
		string file_name = "Datas/"+levelName;
		FileInfo fileInfo;
		string _data;
		if(Application.platform == RuntimePlatform.IPhonePlayer)  
		{  
			fileInfo = new FileInfo(Application.dataPath + "/Raw/" + file_name);   
			StreamReader r = fileInfo.OpenText();   
			_data = r.ReadToEnd();   
			r.Close();   
		}  
//		else if(Application.platform == RuntimePlatform.Android)  
//		{  
////			fileInfo = new FileInfo(Application.streamingAssetsPath+file_name);  
////			StartCoroutine("LoadWWW");  
//		}  
		else  
		{  
			fileInfo = new FileInfo(Application.dataPath + "/StreamingAssets/"+ file_name);   
			StreamReader r = fileInfo.OpenText();   
			_data = r.ReadToEnd();   
			r.Close();   
		}     

//		FileStream file_stream;  
//		string file_path = Application.streamingAssetsPath+"/Datas/"; 
//
//		file_stream = File.OpenRead (file_path + "//" + file_name);
//		int fsLen = (int)file_stream.Length;
//		byte[] heByte = new byte[fsLen];
//		int r = file_stream.Read(heByte, 0, heByte.Length);
//		string myStr = System.Text.Encoding.UTF8.GetString(heByte);	
//		string[] tmpS = myStr.Split ('$');
		string[] tmpS = _data.Split('$');
		layerNum = int.Parse( tmpS [0]);
		seperateNum = int.Parse (tmpS [1]);
		winCond = tmpS [2];
		layerNum = layerNum;
		seperateNum = seperateNum;
		config = new SectorConfig[layerNum * seperateNum];
		for (int i = 3; i < tmpS.Length - 1; i++) {
			string json = tmpS [i];
			config [i - 3] = JsonUtility.FromJson<SectorConfig> (json);
		}
		GetComponent<Plate> ().refresh ();
		//		plate.GetComponent<Plate>().
		string[] sss = winCond.Split(',');
		foreach (var s1 in sss) {
			winMusic.Add (s1);
		}
		setTarget ();
//		file_stream.Close ();
	}
		
	public void getOneNoteDone(string note){
		if (winMusic [0] != null && winMusic[0] == note ) {
			winMusic.RemoveAt (0);
			refreshTargetUI ();
		}
	}

	void setTarget(){
		targetUI.GetComponent<Text> ().text = winCond;
	}

	void refreshTargetUI(){
		if (winMusic.Count > 0) {
			string s = "";
			foreach (var v in winMusic) {
				s += v;
				s += ",";
			}
			targetUI.GetComponent<Text> ().text = s;
		} else {
			targetUI.GetComponent<Text> ().text = "";
			levelNum++;
			if (globalConfig.levelList [levelNum] != null) {
				open (globalConfig.levelList [levelNum]);
				refresh ();
			}
		}

	}
}

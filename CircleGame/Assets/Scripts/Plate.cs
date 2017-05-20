using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class globalConfig{
	public static float radius = 5.0f;
	public static Dictionary<string,string> colorMusicPair = new Dictionary<string, string> (){
		{"242,109,109","do"},
		{"245,138,172","re"},
		{"205,166,228","mi"},
		{"185,122,223","fa"},
		{"105,137,210","so"},
		{"45,197,201","la"},
		{"243,185,28","xi"},
	};
	public static Dictionary<string,string> musicColorPair = new Dictionary<string, string> (){
		{"do","242,109,109"},
		{"re","245,138,172"},
		{"mi","205,166,228"},
		{"fa","185,122,223"},
		{"so","105,137,210"},
		{"la","45,197,201"},
		{"xi","243,185,28"},
	};
	public static List<string> levelList = new List<string> () {
		"2-1", "1-2", "1-3",
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
		new SectorConfig(120f, 0f, 1.0f, 105,137,210),
		new SectorConfig(120f, 120f, 1.0f, 185,122,223),
		new SectorConfig(120f, 240f, 1.0f, 242,109,109),

		new SectorConfig(120f, 25f, 2.0f, 242,109,109),
		new SectorConfig(120f, 145f, 2.0f, 105,137,210),
		new SectorConfig(120f, 265f, 2.0f, 185,122,223),

		new SectorConfig(120f, 45f, 3.0f, 185,122,223),
		new SectorConfig(120f, 165f, 3.0f, 242,109,109),
		new SectorConfig(120f, 285f, 3.0f, 105,137,210),
	};
	public string winCond;
	public List<string> winMusic;
	public GameObject[] sectors;
	public string currentLevel = "2-1";
	public float[] sector_rotations; //记录所有扇形当前的旋转角度，后面就不用计算啦
	public bool isHiddenColorMode = false;
	public void setSectorRotation(int sectorIndex, float rotation)
	{
		sector_rotations [sectorIndex] = rotation;
	}

	public float getSectorRotation(int sectorIndex)
	{
		return sector_rotations [sectorIndex];
	}
		
	public GameObject notePrefab;

	// Use this for initialization
	void Awake () {
		drawGame ();
	}

	void Start(){
		if (Camera.main.GetComponent<EditorControl> () == null) {
			open(currentLevel);
		}
		startMakeNote ();
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
			if (isHiddenColorMode) {
				sectors [i] = DrawTool.DrawSectorSolid (transform, transform.localPosition, c.angle, c.radius, new Color(1,1,1),c.rotation);
				var frame = DrawTool.DrawSector (sectors [i].transform, sectors [i].transform.localPosition, c.angle, c.radius);
//				frame.transform.parent = sectors [i].transform;
//				frame.transform.Rotate (new Vector3(0, 0, c.rotation));
//				frame.transform.Rotate(new Vector3(0,0,100));
			} else {
				sectors [i] = DrawTool.DrawSectorSolid (transform, transform.localPosition, c.angle, c.radius, new Color(c.r/255.0f,c.g/255.0f,c.b/255.0f),c.rotation);
			}

			Vector3 t = sectors [i].transform.position;
			sectors [i].transform.localPosition = new Vector3 (0, 0, c.radius);
			sectors [i].transform.Rotate (new Vector3(0, 0, c.rotation));
			sectors [i].name = "sector_"+ i.ToString();
			sectors [i].AddComponent <SectorControl>();
			sectors [i].GetComponent<SectorControl> ().color = new float[3]{ c.r, c.g, c.b };
			sectors [i].GetComponent<SectorControl>().note =globalConfig.colorMusicPair [c.r + "," + c.g + "," + c.b];



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
		if (isHiddenColorMode) {
//			drawFrontier ();		
		}

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
		if (GetComponent<TouchControl> () != null){
			GetComponent<TouchControl> ().reset ();
		}
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
				config [n] = new SectorConfig (angle, rot, radius, 255, 255, 255);
			}
		}
	}


	public void actionComplete()
	{

	}


	public void alignSector(int circleIndex, int sectorIndex, ActionCallback callback)
	{

//		Debug.Log ("wenkan alignSector");
		float rotation = sector_rotations [sectorIndex];
		rotation = rotation % 360 + 45.0f;

		int startIndex = circleIndex * seperateNum;
		for (int i = startIndex; i < startIndex + seperateNum; i++) {
			float last_rotation = sector_rotations [i];
//			sectors [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, last_rotation - rotation));
			Quaternion to = Quaternion.Euler (new Vector3 (0, 0, last_rotation - rotation));
			var rotateTo = sectors [i].AddComponent<RotateTo> ();
			rotateTo.setParams (to, 0.5f, callback);
			GetComponent<Plate> () .setSectorRotation (i, last_rotation - rotation);
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
			config [i - 3].radius *= 2.4f / 3;
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
		
	public void getOneNoteDone(string note,int[] sectors){
		Debug.Log (note);
		if (winMusic [0] != null && winMusic[0] == note ) {
			Camera.main.GetComponent<WInLoseControl> ().deleteNode ();
			winMusic.RemoveAt (0);
			refreshTargetUI ();
			changeColor (sectors);
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
			Camera.main.GetComponent<WInLoseControl> ().winPanel.SetActive (true);
//			targetUI.GetComponent<Text> ().text = "";
//			levelNum++;
//			if (globalConfig.levelList [levelNum] != null) {
//				open (globalConfig.levelList [levelNum]);
//				refresh ();
//				startMakeNote ();
//			}
		}

	}

	public void changeColor(int[] sectors){
		if (winMusic.Count == 0)
			return;
		string s = winMusic [0];
		string colorS="";
		string[] res = new string[7];
		int n = globalConfig.colorMusicPair.Count ;
		List<string> list = new List<string> ();
		List<string> noteList = new List<string> ();
		int countRes = 0;
		foreach (var i in globalConfig.colorMusicPair) {
			list.Add (i.Key);
			noteList.Add (i.Value);
			if (i.Value == s) {
				colorS = i.Key;
			}
			res[countRes] = i.Key;
			if (s == i.Value) {
				int m = countRes;
				var temp = res [m];
				res [m] = res [0];
				res [0] = temp;
			}
			countRes++;
		}
		for (int i = 0; i < 3; i++) {
			int nn = Random.Range (1, n + 1);
			res [i + 1] = list [nn - 1];
		}
		string[] css = colorS.Split (',');
		foreach(var i in sectors){
			int circleNum = (int)(i / seperateNum);
			bool isHasColor = false;
			for (int j = 0; j < seperateNum; j++) {
				int num = circleNum * seperateNum;
				if (num + j != i) {
					float r = config [num + j].r;
					float g = config [num + j].g;
					float b = config [num + j].b;
					if (float.Parse(css [0]) == r && float.Parse(css [1]) == g && float.Parse(css [2]) == b) {
						isHasColor = true;
					}
				}
			}
			if (isHasColor) {
				int leftIndex = i + 1;
				int rightIndex = i - 1;
				if (i % seperateNum == 0) {
					rightIndex += seperateNum;
				}
				if (i % seperateNum == seperateNum - 1) {
					leftIndex -= seperateNum;
				}
				var c1 = config [leftIndex];
				string cs1 = c1.r.ToString () + "," + c1.g.ToString () + "," + c1.b.ToString ();
				string note1 = globalConfig.colorMusicPair [cs1];
				var c2 = config [rightIndex];
				string cs2 = c2.r.ToString () + "," + c2.g.ToString () + "," + c2.b.ToString ();
				string note2 = globalConfig.colorMusicPair [cs2];
				for (int ii = 0; ii < noteList.Count; ii++) {
					if (noteList [ii] == note1) {
						leftIndex = ii;
					}
					if (noteList [ii] == note2) {
						rightIndex = ii;
					}
				}
				var temp = noteList [6];
				noteList [6] = note1;
				noteList [leftIndex] = temp;
				var temp1 = res [6];
				res [6] = res [leftIndex];
				res [leftIndex] = temp1;

				var temp2 = noteList [5];
				noteList [5] = note2;
				noteList [rightIndex] = temp2;
				var temp3 = res [5];
				res [5] = res [rightIndex];
				res [rightIndex] = temp3;

				int index = Random.Range (0, 5);

				string coolorS = res [index];
				string[] coolorss = coolorS.Split (',');
				config [i].r = float.Parse(coolorss [0]);
				config [i].g = float.Parse(coolorss [1]);
				config [i].b = float.Parse(coolorss [2]);
			}else{
				string coolorS = res [0];
				Debug.Log (coolorS);
				string[] coolorss = coolorS.Split (',');
				config [i].r = float.Parse(coolorss [0]);
				config [i].g = float.Parse(coolorss [1]);
				config [i].b = float.Parse(coolorss [2]);
			}
		}
		refresh ();
	}

	void startMakeNote(){
		float time = 0;
		foreach (var music in winMusic) {
			StartCoroutine (pawnNote (music,time));
			time += 10f;
		}
	}

	IEnumerator  pawnNote(string note,float waitTime){
		yield return new WaitForSeconds(waitTime);
		GameObject obj = Instantiate (notePrefab) as GameObject;
		obj.GetComponent<NoteControl> ().setPos (note);
		Camera.main.GetComponent<WInLoseControl> ().addNote (obj);
	}

	public void playAgain(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void goNextScene(){
		string name = SceneManager.GetActiveScene ().name;
		if (name == "scene2") {
			SceneManager.LoadScene("scene3");
		}
		if (name == "scene3") {
			SceneManager.LoadScene("scene2");
		}
	}
}

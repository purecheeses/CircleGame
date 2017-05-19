using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  
using System.Text;
using UnityEngine.UI;

public class EditorControl : MonoBehaviour {
	public int layerNum = 3;
	public int seperateNum =3;
	public string levelName = "1-1";
	public GameObject layerInput;
	public GameObject seperateInput;
	public GameObject levelInput;
	public Color selectColor;
	public GameObject plate;
	public GameObject colorGroup;
	public SectorConfig[] config = {
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

	void Start(){
		int n = 1;
		foreach (var dic in globalConfig.colorMusicPair) {
			string s = dic.Key;
			string[] ss = s.Split (',');
			var t = colorGroup.transform.FindChild ("Button (" + n.ToString () + ")");
//			Color color = new Color (int.Parse (ss [0]), int.Parse (ss [1]), int.Parse (ss [2]));
//			ColorBlock cb = new ColorBlock();
////			Debug.Log (color);
//			cb.normalColor = color;
//			cb.highlightedColor = Color.white;
//			cb.pressedColor = Color.white;
//			cb.disabledColor = Color.white;
//			t.GetComponent<Button> ().colors = cb;
//			t.GetComponent<Image> ().color = color;
			t.GetComponentInChildren<Text> ().text = s;
			n++;
		}
	}

	public void save (){
		string file_path = Application.dataPath+"/Datas/"; 
		config = plate.GetComponent<Plate> ().config;
		int n = config.Length;
		string res = "";
		res += layerNum.ToString ();
		res += "$";
		res += seperateNum.ToString ();
		res += "$";
		for (int i = 0; i < n; i++) {
			string json = JsonUtility.ToJson (config [i]);
			res += json;
			res += "$";
		}
		FileStream file_stream;  
		string file_name = levelName;
		Debug.Log (file_name);
		file_stream=File.Open(file_path+"//"+file_name,FileMode.Create,FileAccess.Write);//打开现有 UTF-8 编码文本文件以进行读取  
	
		byte[]bytes = Encoding.UTF8.GetBytes(res);
		foreach(byte b in bytes)
		{
			file_stream.WriteByte(b);        //逐个字节逐个字节追加入文本
		}
		file_stream.Flush();
		file_stream.Close ();
	}

	public void recordLevel(string s){
		levelName = levelInput.GetComponent<InputField> ().text;
	}
	public void recordPieceNum(string s){
		string ss = seperateInput.GetComponent<InputField> ().text;
		seperateNum = int.Parse (ss);
		plate.GetComponent<Plate> ().seperateNum = seperateNum;
		plate.GetComponent<Plate> ().makeNewData ();
		plate.GetComponent<Plate> ().refresh ();
	}

	public void recordLayerNum(string s){
		string ss = layerInput.GetComponent<InputField> ().text;
		layerNum = int.Parse (ss);
		plate.GetComponent<Plate> ().layerNum = layerNum;
		plate.GetComponent<Plate> ().makeNewData ();
		plate.GetComponent<Plate> ().refresh ();
	}

	public void open(){
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
		plate.GetComponent<Plate> ().layerNum = layerNum;
		plate.GetComponent<Plate> ().seperateNum = seperateNum;
		plate.GetComponent<Plate> ().config = new SectorConfig[layerNum * seperateNum];
		for (int i = 2; i < tmpS.Length - 1; i++) {
			string json = tmpS [i];
			plate.GetComponent<Plate> ().config [i - 2] = JsonUtility.FromJson<SectorConfig> (json);
		}
		plate.GetComponent<Plate> ().refresh ();
//		plate.GetComponent<Plate>().
	}

	public void setColor(string s ){
		string[] ss = s.Split (',');
		selectColor = new Color(float.Parse(ss[0]),float.Parse(ss[1]),float.Parse(ss[2]));
	}
}

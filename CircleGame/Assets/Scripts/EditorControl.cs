using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  
using System.Text;

public class EditorControl : MonoBehaviour {
	public int layerNum = 3;
	public int seperateNum =3;
	public string levelName = "1-1";
	SectorConfig[] config = {
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

	public void save (){
		string file_path = Application.dataPath+"/Datas/"; 
		int n = config.Length;
		string res = "";
		for (int i = 0; i < n; i++) {
			string json = JsonUtility.ToJson (config [i]);
			res += json;
			res += "$$";
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
		Debug.Log (s);
		levelName = s;
	}
	public void recordPieceNum(string s){
		seperateNum = int.Parse (s);
	}

	public void recordLayerNum(string s){
		layerNum = int.Parse (s);
	}

	public void open(){
		FileStream file_stream;  
		string file_path = Application.dataPath+"/Datas/"; 
		string file_name = levelName;
		file_stream = File.OpenRead (file_path + "//" + file_name);
	}
}

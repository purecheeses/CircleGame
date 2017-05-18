using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtonControl : MonoBehaviour {

	public void onClick(){
		string s = GetComponentInChildren<Text> ().text;
		Camera.main.GetComponent<EditorControl> ().setColor (s);
	}
}

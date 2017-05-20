using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WInLoseControl : MonoBehaviour {
	List<GameObject> list;
	public GameObject winPanel;
	public GameObject losePanel;
	// Use this for initialization
	void Start () {
		list = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void addNote(GameObject obj){
		list.Add (obj);
	}

	public void deleteNode(){
		if (list.Count > 0) {
			GameObject obj = list [0];
			obj.GetComponent<NoteControl> ().FadeOut();
			list.RemoveAt (0);
		}

	}
}

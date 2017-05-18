using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button btn = this.GetComponent<Button> ();
		UIEventListener ul = btn.gameObject.AddComponent<UIEventListener> ();
		string name = btn.name;
		ul.onClick += delegate (PointerEventData eventData, GameObject go) {
			Debug.Log("wenkan onClick");
			GameObject[] circles = Main.getCircles();

			if (name == "innerBtn") {
				Debug.Log("inner button click");
				for (int i=0;i<3;i++) {
					circles[i].transform.Rotate(new Vector3(0,0,1f));
				}
			} else if (name == "middleBtn") {
				Debug.Log("middle button click");
				for (int i=3;i<6;i++) {
					circles[i].transform.Rotate(new Vector3(0,0,-1f));
				}

			} else if (name == "outerBtn") {
				Debug.Log("outer button click");
				for (int i=6;i<9;i++) {
					circles[i].transform.Rotate(new Vector3(0,0,1f));
				}
			}
		};

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}

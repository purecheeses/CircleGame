using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteControl : MonoBehaviour {
	public Vector3 enterPos ;
	public Vector3 beginPos;
	public Vector3 endPos;
	public float enterTime	;
	public float passTime;
	public float deltaY = 0.1f;
	public Dictionary<string,int> noteDic;
	public float deltaAlpha;
	public Vector3 deltaEnter;
	public Vector3 deltaPass;
	bool isFadingOut;
	float fadeOutTime = 1.5f;
	float fadeOutDelta;
	// Use this for initialization

	void Awake(){
		noteDic = new Dictionary<string,int> {
			{"do",0},{"re",1},{"mi",2},{"fa",3},{"so",4},{"la",5},{"xi",6},
		};
		fadeOutDelta = 1f / (60f * fadeOutTime);
	}

	void Start () {
		enterTime = 2f;
		passTime = 20f;
		enterPos = new Vector3 (-1.17f, 3.69f,0f);
		beginPos = new Vector3 (-0.92f, 3.69f,0f);
		endPos = new Vector3 (3.44f, 3.69f,0f);

		init ();
		transform.position = enterPos;
		Color color = GetComponent<SpriteRenderer> ().color;
		GetComponent<SpriteRenderer> ().color = new Color (color.r, color.g, color.b, 0);
		isFadingOut = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (enterTime >= 0f) {
			enterTime -= Time.deltaTime;
			Color color = GetComponent<SpriteRenderer> ().color;
			GetComponent<SpriteRenderer> ().color = new Color (color.r, color.g, color.b, color.a + deltaAlpha);
			transform.Translate (deltaEnter);
		} else {
			if (passTime >= 0) {
				passTime -= Time.deltaTime;
				transform.Translate (deltaPass);
			} else {
				GameOver ();
			}
		}
		if (isFadingOut) {
			Color color = GetComponent<SpriteRenderer> ().color;
			GetComponent<SpriteRenderer> ().color = new Color (color.r, color.g, color.b, color.a -fadeOutDelta);
		}
	}

	void GameOver(){
	}

	public void setPos(string note){
		transform.position = enterPos+ new Vector3 (0f, noteDic [note] * deltaY, 0f);
		enterPos += new Vector3 (0f, noteDic [note] * deltaY, 0f);
		beginPos += new Vector3 (0f, noteDic [note] * deltaY, 0f);
		endPos += new Vector3 (0f, noteDic [note] * deltaY, 0f);
		init ();;
	}
	void init(){
		deltaAlpha = 1f / (enterTime * 60f);
		deltaEnter = (beginPos - enterPos) /(60f* enterTime);
		deltaPass = (endPos - beginPos) / (60f * passTime);
	}

	public void FadeOut(){
		isFadingOut = true;
	}
}

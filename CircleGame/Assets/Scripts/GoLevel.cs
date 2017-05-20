using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void goScene1(){
		SceneManager.LoadScene("scene2");
	}
	public void goScene2(){
		SceneManager.LoadScene("scene3");
	}
	public void goMainScene(){
		SceneManager.LoadScene("MainScene");
	}
}

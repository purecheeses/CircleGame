using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnotherCat : MonoBehaviour {
	public string[] catAnimName = new string[5]{
		"CatIdle","CatTang","CatEat","CatPa","todo2"	
	};
	public string[] catAnim2Name = new string[5]{
		"","CatTang2","CatEat2","CatPa2",""	
	};
	Animator _animator;
	string currentAnimName;
	int loveLevel = 1;
	int loveValue = 0;
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator> ();
		_animator.Play (catAnimName [0]);
		currentAnimName = catAnimName [0];
	}
	
	// Update is called once per frame
	void Update () {
		if (isDone ()) {
			getNextAnim ();
		}
		checkClick ();
	}

	void checkClick(){
		if (Input.GetMouseButton (0)) {
			//好感度计算
				if (loveLevel == 1) {
					if (currentAnimName == catAnimName [1]) {
						if (Random.Range (1, 100) <= 70) {
							addLove (5);
						}
					}
					if (currentAnimName == catAnimName [2]) {
						if (Random.Range (1, 100) <= 50) {
							minusLove (3);
						}
					}
				}
				if (loveLevel == 2) {
					if (currentAnimName == catAnimName [1]) {
						if (Random.Range (1, 100) <= 70) {
							addLove (5);
						}
					}
				}
				if (currentAnimName == catAnimName [2]) {
					minusLove (3);
				}
			//进入动画2
			int index=0;
			for(int i = 0; i < catAnimName.Length-1 ; i++){
				if (catAnimName [i] == currentAnimName) {
					index = i;
					break;
				}
			}
			if (catAnim2Name [index] != "") {
				play (catAnim2Name [index]);
			}
		}
	}

	void minusLove(int num){
		if (loveValue - num >= 0) {
			loveValue -= num;
		} else {
			loveValue = 0;
		}
		checkLoveLevel ();
	}

	void checkLoveLevel(){
		if (loveValue <= 10) {
			loveLevel = 1;
		} else if (loveValue <= 20) {
			loveLevel = 2;
		} else if(loveValue <= 50){
			loveLevel = 3;
		}
	}

	void addLove(int num){
		if (loveValue + num <= 50) {
			loveValue += num;
		} else {
			loveValue = 50;
		}
		checkLoveLevel ();
	}

	bool isDone(){
		AnimatorStateInfo animatorInfo; 
		animatorInfo = _animator.GetCurrentAnimatorStateInfo (0); 
		if ((animatorInfo.normalizedTime > 1)&& (animatorInfo.IsName(currentAnimName))) {
			return true;
		}
		return false;
	}

	void play(string name ){
		_animator.Play (name);
		currentAnimName = name;
	}

	void getNextAnim(){
		int n = Random.Range (0, 100);
		List<int> loveDatas ;
		if (loveLevel == 1) {
			loveDatas = globalConfig.love1;
		} else if (loveLevel == 2) {
			loveDatas = globalConfig.love2;
		} else {
			loveDatas = globalConfig.love3;
		}
		int index = choseRandom (loveDatas, n);
		play (catAnimName [index]);
	}

	int choseRandom(List<int> list , int n){
		List<int> total = new List<int> ();
		int sum = 0;
		foreach (var i in list) {
			sum += i;
			total.Add (sum);
		}
//		Debug.Log (total.Count);
		for (int i = 0; i < total.Count; i++) {
			if (i == 0) {
				if (n <= total [i]) {
					return i;
				}
				if (n > total [i] && n <= +total [i + 1]) {
					return i + 1;
				}
			} else  {
				if (n >= total [i-1] && n<=total[i]) {
					return i;
				}
			} 
		}
		Debug.Log (n);
		return -1;
	}

}

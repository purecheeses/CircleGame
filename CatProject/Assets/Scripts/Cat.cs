using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {

    //初始化
    private Rigidbody2D rigibody;
    private BoxCollider2D collider;
    private Animation _animation;
    private Animator _animator;


    //好感度
    public float love = 0;

    //public float restTime = 1;
    //public float restTimer = 0;

    //状态机控制
    //Idle切换至其他状态
    bool IdleToTang = false;
    bool IdleToPa = false;
    bool IdleToEat = false;

    //Tang切换至其他状态
    bool TangToIdle = false;
    bool TangToPa = false;
    bool TangToEat = false;

    //Pa切换至其他状态
    bool PaToIdle = false;
    bool PaToTang = false;
    bool PaToEat = false;

    //Eat切换至其他状态
    bool EatToIdle = false;
    bool EatToTang = false;
    bool EatToPa = false;

    // Use this for initialization
    void Start () {
        rigibody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
		_animator = GetComponent<Animator>();
//		_animation = GetComponent<Animator> ().animation;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        int randomChoose = Random.RandomRange(1, 100);
        if(love <= 0 )
        {
            if(randomChoose > 50)
            {
				_animator.Play("CatTang");
            }
            if (randomChoose < 50 && randomChoose > 10)
            {
                
            } 
        }
        
	}
}

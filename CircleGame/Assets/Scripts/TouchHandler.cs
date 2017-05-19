using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;


public class TouchHandler
{
	public TouchHandler (Vector2 circleCenter)
	{
		this._circleCenter = circleCenter;
	}

	private Vector2 _circleCenter;

	public delegate void touchBeginProxy(Vector2 startPos);
	public delegate void touchMoveProxy(Vector2 curPos, float curAngle);
	public delegate void touchEndProxy(Vector2 endPos);
	public delegate void touchStationaryProxy(Vector2 curPos);

	public event touchBeginProxy onTouchBegan;
	public event touchMoveProxy onTouchMove;
	public event touchEndProxy onTouchEnd;
	public event touchStationaryProxy onTouchStationary;


	private Vector2 _start_vector;
	private Vector2 _last_pos;
	private float _cumulative_angle = 0.0f;


	#if UNITY_EDITOR
	private Vector3 last_mouse_pos;
	#endif

	public void onUpdate()
	{

		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)) {
			last_mouse_pos = Input.mousePosition;
			touchBeganHandler(last_mouse_pos);
		} else if (Input.GetMouseButton(0)) {
			Vector3 current_pos = Input.mousePosition;
			touchMoveHandler(Input.mousePosition, current_pos-last_mouse_pos);
			last_mouse_pos = current_pos;

		} else if (Input.GetMouseButtonUp(0)){
			touchEndHandler(Input.mousePosition);
		}

		#endif

		#if UNITY_IPHONE
		if (Input.touchCount > 1 || Input.touchCount <= 0)
		{
			return;
		}



		Touch t = Input.touches[0];
		if (t.phase == TouchPhase.Began) {
			touchBeganHandler (t.position);
		} else if (t.phase == TouchPhase.Moved) {
			touchMoveHandler (t.position, t.deltaPosition);
		} else if (t.phase == TouchPhase.Ended) {
			touchEndHandler (t.position);

		} else if (t.phase == TouchPhase.Canceled) {
		} else if (t.phase == TouchPhase.Stationary) {
			touchStationaryHandler (t.position);
		}
		#endif


	}

	void touchBeganHandler(Vector2 startPos)
	{
		this._start_vector = new Vector2 (startPos.x - this._circleCenter.x, 
			startPos.y - this._circleCenter.y);
		this._last_pos = startPos;
		this._cumulative_angle = 0.0f;
		
		if (onTouchBegan != null) 
		{
			onTouchBegan (startPos);
		}
	}

	private Vector3 getVector3ToCenter(Vector2 pos)
	{
		return new Vector3 (pos.x - this._circleCenter.x, pos.y - this._circleCenter.y, 0);
	}

	void touchMoveHandler(Vector2 curPos, Vector2 deltaPos)
	{
		Vector3 last_vector = this.getVector3ToCenter (this._last_pos);
		Vector3 cur_vector = this.getVector3ToCenter (curPos);
		int sign = 1;
		if (Vector3.Cross (cur_vector, last_vector).z < 0) { // 逆时针
//			Debug.Log ("wenkan counter clockwise");
			sign = -1;
		} else {
//			Debug.Log ("wenkan clockwise");
		}

		this._last_pos = curPos;

		float angle = Vector2.Angle (last_vector, cur_vector);
		this._cumulative_angle += sign * angle;
		if (onTouchMove != null) {
			onTouchMove (curPos, this._cumulative_angle);
		}


	}

	void touchEndHandler(Vector2 endPos)
	{
		this._start_vector = new Vector2();
		if (onTouchEnd != null) {
			onTouchEnd (endPos);
		}
	}

	void touchStationaryHandler (Vector2 curPos)
	{
		if (onTouchStationary != null) {
			onTouchStationary (curPos);
		}
	}

	public void onStart()
	{

	}
}



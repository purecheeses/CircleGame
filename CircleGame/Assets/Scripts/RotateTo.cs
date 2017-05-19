using System;
using UnityEngine;

public delegate void ActionCallback();

public class RotateTo : MonoBehaviour
{
	Quaternion _to;
	float _time;
	ActionCallback _cb;

	float _cumutive_time;
	bool _done = false;

	public void setParams(Quaternion to, float time, ActionCallback callback)
	{
		_to = to;
		_time = time;
		_cb = callback;
	}

	void Update()
	{
		if (_done) {
			Destroy (gameObject.GetComponent<RotateTo> ());
			return;
		}
		transform.rotation = Quaternion.Slerp( transform.rotation, _to, _cumutive_time / _time);
		_cumutive_time += Time.deltaTime;
		if (_cumutive_time >= _time && _done == false) {
			_done = true;
			if (_cb != null) {
				_cb ();
			}
		}
	}
}


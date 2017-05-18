using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SectorConfig {
	public SectorConfig(float angle, float rotation, float radius, Color c) {
		this.angle = angle;
		this.radius = radius;
		this.c = c;
		this.rotation = rotation;
	}
	public float angle;
	public float radius;			
	public Color c;
	public float rotation;
}

public class Plate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour {

	public float amplitude = 2;
	public float freq = 2;
	Vector3 pos;

	private void Start() {
		pos = transform.position;
	}

	void Update() {
		transform.position = pos + new Vector3(0,0,Mathf.Sin(Time.time * freq)) * amplitude;
	}
}

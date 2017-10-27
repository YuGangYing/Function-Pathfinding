using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	void Start () {
		
	}

	float smooth = 0.01f;
	void Update () {
		transform.LookAt (target);
		transform.position = new Vector3 (Mathf.Lerp(transform.position.x,target.position.x-2,smooth),transform.position.y,Mathf.Lerp(transform.position.z,target.position.z-2,smooth));
	}
}

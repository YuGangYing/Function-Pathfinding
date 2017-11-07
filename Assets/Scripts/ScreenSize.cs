using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		float defaultSizeRatio = 1920f / 1080f;
//		float currentSizeRatio = Screen.width / Screen.height;
//		float detalRatio = (defaultSizeRatio - currentSizeRatio) / 2f;
//		Camera.main.orthographicSize = Camera.main.orthographicSize * (1 + detalRatio);
		Screen.SetResolution(1920/2,1080/2,false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (LevelBuilder.lb.gameReady) {
			//if (GameManager.gm.twoPlayers == true) {
				Camera[] cameras = FindObjectsOfType<Camera> ();
				if (cameras.Length == 1) {
					cameras [0].rect = new Rect (0, 0, 1, 1);
				} else if (cameras.Length == 2) {
					cameras [0].rect = new Rect (0, 0, 1, 0.499f);
					cameras [1].rect = new Rect (0, 0.502f, 1, 0.499f);
				}
			//}
		}
	}
}

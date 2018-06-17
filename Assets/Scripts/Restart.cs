using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

	public GameObject[] toDestroy;


	// Use this for initialization
	void Start () {
		if (toDestroy == null) {													//if toDestroy is null
			toDestroy = GameObject.FindGameObjectsWithTag ("destroyAtEnd");			//find all gameObjects with destroyAtEnd tag
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RestartGame() {
		for (int i = 0; i < toDestroy.Length; i++) {	//for each object in toDestroy array
			Destroy (toDestroy [i].gameObject);			//destroy that game object
		}

		Destroy(GameObject.Find("GameManager"));		//destroy gameManager
		SceneManager.LoadScene ("StartScene");			//load start scene
	}
}

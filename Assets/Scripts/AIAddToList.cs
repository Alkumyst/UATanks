using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAddToList : MonoBehaviour {



	void Start() {

		GameManager.gm.AIController.Add (this.gameObject);	//adds AI to AIContoller list in game manager
	}
}
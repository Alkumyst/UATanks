using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour {

	public bool isMakingNoise = false;	//tank noise
	private CharacterController cc;		//character controller data

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();	//sets cc to character controller component
	}
	
	// Update is called once per frame
	void Update () {
		if (cc.velocity.magnitude > 0) {	//if player is moving
			isMakingNoise = true;			//it is making noise
		} else {							//if not moving
			isMakingNoise = false;			//it is not making noise		
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]

public class TankMover : MonoBehaviour {

	//Private objects
	private TankData td;																			//tank data

	// Use this for initialization
	void Start () {
		td = GetComponent<TankData> ();																//sets td to tank data
	}

	public void Move ( Vector3 speedAndDirection ) {												//controller says move
		td.controller.SimpleMove ( speedAndDirection );												//move in direction recieved by controller
	}

	public void Rotate ( Vector3 rotationSpeed) {													//controller says rotate
		td.tf.Rotate (rotationSpeed);																//rotate in direction recieved by controller
	}

	public void RotateTowards (Vector3 newDirection) {
		Quaternion goalRotation;																	//create new variable for how we want to end up turned
		goalRotation = Quaternion.LookRotation (newDirection);										//set variable to "how we need to turn in order to look down newDirection"
		td.tf.rotation = Quaternion.RotateTowards (td.tf.rotation, goalRotation, td.turnSpeed);		//rotate just a little from current rotation towards our target rotation
	}
}
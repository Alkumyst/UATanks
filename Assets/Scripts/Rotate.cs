using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	private TankData td;						//tank data 
	public float turnSpeedY;					//turn speed in y direction
	public float turnSpeedX;					//turn speed in x direction
	public float turnSpeedZ;					//turn speed in z direction
	 

	// Use this for initialization
	void Start () {
		td = GetComponent<TankData> ();			//sets td to tank data
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rotationSpeed = new Vector3 (turnSpeedX, turnSpeedY, turnSpeedZ);	//set turn data
		td.tf.Rotate (rotationSpeed);												//rotate upon set vector
	}
}

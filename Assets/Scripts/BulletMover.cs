using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour {

	//Public objects
	public float bulletLife = 2f;			//length of time a bullet will persist in world

	//Private objects
	private TankShooter ts;					//tank shooter data

	// Use this for initialization
	void Start () {
		ts = GetComponent<TankShooter> ();	//sets ts to tank shooter data

		Destroy (gameObject, bulletLife);	//destroys bullet after set amount of time
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// errors out when I try to use this function. bullet moves from code within tank shooter script
	public void BulletMove(Vector3 moveForce){
		Rigidbody tempRigidBody;
		Debug.Log ("MOVE");
		tempRigidBody = ts.temp.GetComponent<Rigidbody> ();
		tempRigidBody.AddForce (moveForce);
	}
}
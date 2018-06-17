using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : MonoBehaviour {

	//Public objects
	public GameObject pfBullet;				//bullet prefab
	public float timeRemaining = 0;			//bullet fire countdown
	[HideInInspector]
	public GameObject temp;					//bullet temp object

	public AudioSource bulletShot;			//bullet firing sound

	//Private objects
	private TankData data;					//tank data

	// Use this for initialization
	void Start () {
		data = GetComponent<TankData> ();	//sets tank data to data
	}
	
	// Update is called once per frame
	void Update () {
		timeRemaining -= Time.deltaTime;	//subtract framedraw time from countdown

	}

	public void Shoot () {																				//controller calles shoot
		if(timeRemaining <= 0) {																		//if countdown is at or under 0 bullet may fire
			Vector3 firePosition;																		//vector3 store for firing tank's position
			firePosition = data.tf.position;															//sets position data
			firePosition = firePosition + (data.tf.forward); 											//moves data point forward in front of tank
			GameObject temp = Instantiate (pfBullet, firePosition, Quaternion.identity) as GameObject; 	//spwans bullet into environment
			bulletShot.Play();																			//play sound
			//bullet mover, I tried to place this in a separate script but kept running into nullReferenceException errors
			Rigidbody tempRigidBody; 
			tempRigidBody = temp.GetComponent<Rigidbody> ();
			tempRigidBody.AddForce (data.tf.forward * data.shellForce);									//moves bullet forward

			DamageOnHit tempBullet;
			tempBullet = temp.GetComponent<DamageOnHit> ();
			tempBullet.tank = data;
		/*	bulletForce = data.tf.forward * data.shellForce;
			SendMessage ("BulletMove", bulletForce, SendMessageOptions.DontRequireReceiver);
		*/
			timeRemaining = data.fireRate; 																//resets timer
		}
	}
}
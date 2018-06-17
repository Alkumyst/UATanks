using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : Pickup {

	public HealthPowerup powerupData;

	public PickupSound pus;

	// Use this for initialization
	void Start () {
		pus = GameObject.FindObjectOfType (typeof(PickupSound)) as PickupSound;		//find pickupSound gameObject w/ script
	}

	public override void OnTriggerEnter (Collider other) {

		pus.playPickupSound();											//play pickup sound
		PowerupManager puManager;
		puManager = other.gameObject.GetComponent<PowerupManager> ();	//get powerup manager from other
		if (puManager != null) {										//if it has one (is not null) then use powerup data
			puManager.powerups.Add (powerupData);						//add powerup to list
			powerupData.ApplyPowerup (puManager.data);					//apply powerup to same TankData as powerup manager
			powerupData.RemovePowerup(puManager.data);					//removes powerup
			Destroy(gameObject);										//destroys the powerup pickup object after it is used after sound plays
		}
	}
}

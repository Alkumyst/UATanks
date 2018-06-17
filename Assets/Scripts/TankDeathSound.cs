using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDeathSound : MonoBehaviour {

	public AudioSource tankDeath;

	public void playDeathSound() {
		tankDeath.Play ();				//play sound
	}



}

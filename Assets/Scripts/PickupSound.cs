using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSound : MonoBehaviour {

	public AudioSource pickup;

	public void playPickupSound() {
		pickup.Play ();					//play sound
	}

}

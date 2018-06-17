using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

	public TankData data;								//tank data

	public List<Powerup> powerups;						//powerup list


	// Use this for initialization
	void Start () {
		data = GetComponent<TankData> ();				//sets data to tank data
	}


	public void HandlePowerups () {
		List<Powerup> powerupsToRemove = new List<Powerup> ();
		foreach (Powerup pu in powerups) {			//move through list of powerups
			pu.lifespan -= Time.deltaTime;			//subtract from lifespan
			if (pu.lifespan < 0){					//if lifespan is less than 0
				pu.RemovePowerup(data);				//apply RemovePowerup function
				powerupsToRemove.Add (pu);			//add to remove list
			}
		}
		foreach (Powerup pu in powerupsToRemove) {	//now list is full, remove each item in list
			powerups.Remove (pu);					//remove pu from lsit
		}
	}

	// Update is called once per frame
	void Update () {
		HandlePowerups ();
		if (Input.GetKeyDown (KeyCode.P)) {				//if "p" is pressed down
			Test ();									//run Test function
		}
	}

	void Test () {										//test function for powerup testing
		/*powerups = new List<Powerup> ();				//sets space for new powerup in powerup list
		HealthPowerup temp = new HealthPowerup();		//temp for health powerup
		temp.displayName = "Test Health";				//temp health displayName
		temp.healthToAdd = 100;							//temp health healthToAdd

		powerups.Add (temp);							//apply powerup in list

		DefencePowerup temp2 = new DefencePowerup ();	//temp for defence powerup
		temp2.displayName = "Increased Defence";		//temp defence displayName
		temp2.defenceBoost = 3;							//temp defence defenceBoost

		powerups.Add (temp2);							//apply powerup in list

		//method 1 for applying powerups
		//temp.ApplyPowerup (data);
		//temp2.ApplyPowerup (data);

		//method 2
		foreach (Powerup pu in powerups) {				//for each item in list
			pu.ApplyPowerup (data);						//run ApplyPowerup function
		}
		*/

	}

}

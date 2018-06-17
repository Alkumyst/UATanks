using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToList : MonoBehaviour {

	//variable definitions
	public bool isPlayerSpawn;
	public bool isPatrol;
	public bool isPickup;
	public bool isRoom;
	public bool isAISpawn;

	void Start() {
		if (isPlayerSpawn) {											//if isPlayerSpawn is true
			GameManager.gm.playerSpawn.Add (this.gameObject);			//add it to playerSpawn list in game manager
		} else if (isPatrol) {											//if isPatrol is true
			GameManager.gm.AIPatrol.Add (this.gameObject.transform);	//add it to AIPatrol list in game manager
		} else if (isPickup) {											//if isPickup is true
			GameManager.gm.powerupsList.Add (this.gameObject);			//add it to powerupList list in game manager
		} else if (isRoom) {											//if isRoom is true
			GameManager.gm.roomsInUse.Add (this.gameObject);			//add it to roomsInUse list in game manager
		} else if (isAISpawn) {											//if isAISpawn is true
			GameManager.gm.AISpawn.Add (this.gameObject);				//add it to AISpawn list in game manager
		}

	}
}

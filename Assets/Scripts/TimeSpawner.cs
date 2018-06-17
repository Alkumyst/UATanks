using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpawner : MonoBehaviour {

	public GameObject spawnPrefab;				//prefab for what to spawn
	public float respawnTime;					//how long until spawn
	public bool spawnOnLoad;					//will it spawn on load
	private GameObject spawnedObject;			//object after its spawned
	public float countdown;						//time till respawn
	private Transform tf;

	// Use this for initialization
	void Start () {
		tf = GetComponent<Transform> ();		//set tf to Transform component
		if (spawnOnLoad) {						//if spawn on start
			Spawn ();							//spawn
		}
		countdown = respawnTime;				//set countdown to respawnTime
	}
	
	// Update is called once per frame
	void Update () {
		if (spawnedObject == null) {			//spawned object is null if we dont have object spawned
			countdown--;						//remove 1 from countdown every frame
		}
		if (countdown == 0) {					//if countdown = 0 spawn
			Spawn ();							//spawn
		}	
	}

	void Spawn () {
		spawnedObject = Instantiate (spawnPrefab, tf.position, tf.rotation) as GameObject;	//spawn object
		countdown = respawnTime;															//reset timer
	}
}

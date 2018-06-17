using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//Public objects
	[Header("Keys")]
	public KeyCode forwardKey = KeyCode.W;			//key to make tank move forward
	public KeyCode reverseKey = KeyCode.S;			//key to make tank move reverse
	public KeyCode turnClockKey = KeyCode.D;		//key to make tank turn clockwise
	public KeyCode turnCounterClockKey = KeyCode.A;	//key to make tank turn counterclockwise
	public KeyCode fireBullet = KeyCode.Space;		//key to fire a bullet
	public KeyCode mapView = KeyCode.M;				//key to view map
	public KeyCode exitGame = KeyCode.Escape;

	//Private objects
	private TankData data;							//tank data
	private TankShooter ts;							//tank shooter

	void Awake () {
		if (tag == "player") {
			GameManager.gm.player.Insert (0, this);							//player is set in game manager
			GameManager.gm.inGamePlayer.Insert (0, this.gameObject);		//player is set in game manager
		} else if (tag == "player2") {
			GameManager.gm.player.Insert (1, this);							//player is set in game manager
			GameManager.gm.inGamePlayer.Insert (1, this.gameObject);		//player is set in game manager
		}
	}

	void OnDestroy () {
		if (tag == "player") {
			GameManager.gm.player.RemoveAt (0);						//player is set in game manager
			GameManager.gm.inGamePlayer.RemoveAt (0);				//player is set in game manager
		} else if (tag == "player2") {
			GameManager.gm.player.RemoveAt (1);						//player is set in game manager
			GameManager.gm.inGamePlayer.RemoveAt (1);				//player is set in game manager
		}
	}

	// Use this for initialization
	void Start () {
		data = GetComponent<TankData> ();					//sets data to tank data
		ts = GetComponent<TankShooter> ();					//sets ts to tank shooter

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveVector = Vector3.zero;						//Create our variable, set no movement
		Vector3 turnVector = Vector3.zero;						//Create our variable, set no movement
		if (Input.GetKey (forwardKey)) {						//forward key is pressed
			moveVector = data.tf.forward * data.forwardSpeed;	//set forward data
		} else if (Input.GetKey (reverseKey)) {					//reverse key is pressed
			moveVector = data.tf.forward * -data.reverseSpeed;	//set reverse data
		} 
		if (Input.GetKey (turnClockKey)) {						//clockwise key is pressed
			turnVector = new Vector3 (0, data.turnSpeed, 0);	//set clockwise data
		} else if (Input.GetKey (turnCounterClockKey)) {		//counterclockwise key is pressed
			turnVector = new Vector3 (0, -data.turnSpeed, 0);	//set counterclockwise data
		} 
		if (Input.GetKey (fireBullet)) {						//fire bullet button is pressed
			ts.Shoot ();										//fire bullet
		}
		if (Input.GetKey (mapView)) {
			//TODO: add map view
		}
		if (Input.GetKey (exitGame)) {
			Application.Quit ();
		}
		//actually move
		SendMessage("Move", moveVector, SendMessageOptions.DontRequireReceiver);	//tell TankMover to move in direction from pressed key
		SendMessage("Rotate", turnVector, SendMessageOptions.DontRequireReceiver);	//tell TankMover to rotate in direction from pressed key
	}
}


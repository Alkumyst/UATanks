using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class TankData : MonoBehaviour {
	//Public objects
	[HideInInspector]
	public Transform tf;					//transform data
	[HideInInspector]
	public CharacterController controller;	//character controller data

	[Header("Tank Type")]
	public bool playerOne = false;
	public bool playerTwo = false;
	public bool hunter = false;
	public bool defender = false;
	public bool patroller = false;
	public bool fleer = false;

	[Header("Movement")]
	public float forwardSpeed = 5;		//tank's forward speed
	public float reverseSpeed = 5;		//tank's reverse speed
	public float turnSpeed = 5;			//tank's rotational speed

	[Header("Combat")]
	public float shellForce = 600; 		//speed of bullet
	public float damageToTake = 10; 	//amount of damage this tank recieves when hit
	public float fireRate = 1;			//amount of time before next shot
	public float health = 100;			//tank's health
	public float maxHealth = 500;		//tank max health
	[Header("Score Data")]
	public float hunterScoreVal = 10;		//score value
	public float defenderScoreVal = 10;		//score value
	public float patrollerScoreVal = 10;	//score value
	public float fleerScoreVal = 10;		//score value
	public float playerScorVal = 10;		//score value


	public bool displayTank = false;

	public void Awake () {
		tf = GetComponent<Transform> ();					//sets tf to transform component
		controller = GetComponent<CharacterController> ();	//sets controller to character controller component
	}

	void Update () {			//continuously send HUD updates for player 1 and player 2  
		if (!displayTank) {
			if (tag == "player") {
				SendMessage ("healthUpdate", health, SendMessageOptions.DontRequireReceiver);					
				SendMessage ("scoreUpdate", GameManager.gm.score1, SendMessageOptions.DontRequireReceiver);
				SendMessage ("livesUpdate", GameManager.gm.lives, SendMessageOptions.DontRequireReceiver);
			}
			if (tag == "player2") {
				SendMessage ("healthUpdate", health, SendMessageOptions.DontRequireReceiver);
				SendMessage ("scoreUpdate", GameManager.gm.score2, SendMessageOptions.DontRequireReceiver);
				SendMessage ("livesUpdate", GameManager.gm.p2Lives, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void TakeDamage (float damageDone) {						//tank was hit
		health = health - damageDone;								//take damage
	}
}
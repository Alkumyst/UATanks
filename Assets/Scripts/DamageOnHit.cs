 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]

public class DamageOnHit : MonoBehaviour {

	//Private objects
	private TankData data;			//tank data
	private float damageTaken;		//damage taken from TankData
	public TankData tank;			//tankData for the tank who shot

	public TankDeathSound tds;

	// Use this for initialization
	void Start () {
		tds = GameObject.FindObjectOfType (typeof(TankDeathSound)) as TankDeathSound;		//find TankDeathSound gameObject w/ script
	}

	void OnTriggerEnter (Collider other) {																																//when bullet hits trigger hitbox
		if (other.tag == "player" || other.tag == "player2" || other.tag == "hunter" || other.tag == "defender" || other.tag == "fleer" || other.tag == "patroller") {	//if bullet hits another tank
			data = other.gameObject.GetComponent<TankData> ();																											//data recieves TankData info from targeted hitbox collider
			damageTaken = data.damageToTake;																															//damageTaken recieves damageToTake data
			other.gameObject.SendMessage ("TakeDamage", damageTaken, SendMessageOptions.DontRequireReceiver);															//tells targeted hitbox collider to activate the TakeDamage function with damageTaken as the argument


			if (tank.tag == "player") {													//if tank who shot is player
				if (data.health <= 0) {													//and if tank hit health is less than 0
					if (data.hunter) {													//if hit tank is hunter
						GameManager.gm.score1 += data.hunterScoreVal;					//add hunter score value to player 1 score in gameManager
						Debug.Log("hunter score");	
					}
					if (data.defender) {												//if hit tank is defender
						GameManager.gm.score1 += data.defenderScoreVal;					//add defender score value to player 1 score in gameManager
						Debug.Log("defender score");
					}
					if (data.patroller) {												//if hit tank is patroller
						GameManager.gm.score1 += data.patrollerScoreVal;				//add patroller score value to player 1 score in gameManager
						Debug.Log("patroller score");
					}
					if (data.fleer) {													//if hit tank is fleer
						GameManager.gm.score1 += data.fleerScoreVal;					//add fleer score value to player 1 score in gameManager
						Debug.Log("fleer score");
					}
					if (data.playerTwo) {												//if hit tank is player 2
						GameManager.gm.score1 += data.playerScorVal;					//add player score value to player 1 score in gameManager
					}
				}
			}
			if (tank.tag == "player2") {												//if tank who shot is player 2
				if (data.health <= 0) {													//and if tank hit health is less than 0
					
					if (data.hunter) {													//if hit tank is hunter
						GameManager.gm.score2 += data.hunterScoreVal;					//add hunter score value to player 2 score in gameManager
					}
					if (data.defender) {												//if hit tank is defender
						GameManager.gm.score2 += data.defenderScoreVal;					//add defender score value to player 2 score in gameManager
					}
					if (data.patroller) {												//if hit tank is patroller
						GameManager.gm.score2 += data.patrollerScoreVal;				//add patroller score value to player 2 score in gameManager
					}
					if (data.fleer) {													//if hit tank is fleer
						GameManager.gm.score2 += data.fleerScoreVal;					//add fleer score value to player 2 score in gameManager
					}
					if (data.playerOne) {												//if hit tank is player 1
						GameManager.gm.score2 += data.playerScorVal;					//add player score value to player 2 score in gameManager
					}
				}
			}

			if (data.health <= 0) {																			//if no more health
				Debug.Log("TANK DEAD");
				tds.playDeathSound();																		//play death sound
				Destroy (other.gameObject);																	//destroy tank after wait time for sound 
				if (data.gameObject.CompareTag ("player")) {												//if tank has player tag
					GameManager.gm.playerAlive = false;														//tell game manager player is not alive
					GameManager.gm.lives--;																	//reduce player 1 lives by 1
				} else if (data.gameObject.CompareTag ("player2")) {										//if tank has player2 tag
					GameManager.gm.player2Alive = false;													//tell game manager player is not alive
					GameManager.gm.p2Lives--;																//reduce player 2 lives by 1
				} else if (data.gameObject.CompareTag  ("hunter")) {										//if tank has hunter tag
					GameManager.gm.hunterAlive = false;														//tell game manager hunter is not alive
				} else if (data.gameObject.CompareTag ("defender")) {										//if tank has defender tag
					GameManager.gm.defenderAlive = false;													//tell game manager defender is not alive
				} else if (data.gameObject.CompareTag ("fleer")) {											//if tank has fleer tag
					GameManager.gm.fleerAlive = false;														//tell game manager fleer is not alive
				} else if (data.gameObject.CompareTag ("patroller")) {										//if tank has patroller tag
					GameManager.gm.patrollerAlive = false;													//tell game manager patroller is not alive
				}
			} 
			Debug.Log("TANK HIT");
		} else {} 																							//do nothing else
		Destroy (gameObject);																				//destroy bullet
	}

	void OnCollisionEnter (Collision collision) {	//when bullet collides with a non-trigger
		if (collision.gameObject.tag == "wall") {	//if collided object has tag set to "wall"
			Destroy (gameObject);					//destroy bullet
		}
	}
}
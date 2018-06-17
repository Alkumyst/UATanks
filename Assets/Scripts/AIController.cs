using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStates { idle, patrol, shoot, patrolAndShoot, seekPlayer, seekAndShoot, fleePlayer };	//AI states
public enum AIAvoidanceStates { normal, turnToAvoid, moveToAvoid };									//obstacle avoidance states
public enum LoopTypes { stop, loop, pingpong, random };												//patrol states
public enum PersonalityType  { hunter, fleer, patroller, defender };								//AI personalities

public class AIController : MonoBehaviour {

	public AIStates AIState;			//AI state
	public List<GameObject> target;		//enemy target
	public float lastStateChangeTime;	//how long has it been since last state change
	[HideInInspector]
	public TankData data;				//tank data

	[Header("Personality Data")]
	public PersonalityType AIPersonality;		//AI personality type
	public float waitTime = 5.0f;				//how long will the AI wait
	public float defenderHuntTime = 5.0f;		//how long will defender AI hunt target
	public float defenderPatrolTime = 7.0f;		//how long does defender have to return to its waypoint
	public float shootingWaitTime = 1.0f;		//how long will an AI stay in the shoot state when personality is not fleer
	public float fleerFleeTime = 3.0f;			//how long with the fleer personality flee for
	public float fleerAttackRange = 10.0f;		//attack range for fleer personality
	public float patrollerAttackRange = 12.0f;	//attack range for patroller personality
	public float patrollerAttackTime = 5.0f;	//how long will patroller stay on the attack
	[Header("Patrol Data")]
	public LoopTypes loopType;					//patrol loop (what to do at end of patrol)
	public List<Transform> waypoints;			//patrol waypoints
	public int currentWaypoint;					//current waypoint goal
	public float proximity = 1.0f; 				//close enough to waypoint to count as there
	public bool isPatrollingForward = true;		//patrol direction
	public float idleWaitTime = 5.0f;			//how long will hunter and fleer personalities stay in idle state
	[Header("Sense Data")]
	public float viewDistance = 10.0f;			//how far forward can the AI "see"
	public float FOV = 60.0f;					//angle of field of view for AI "sight"
	public bool canSeePlayer = false;			//can the AI see player
	public bool canHearPlayer = false;			//can the AI hear player
	public float hearingRadius = 5.0f;			//hearing radius for AI
	public int i;
	[Header("Flee Data")]
	public float fleeDistance = 5.0f;			//how far will an AI flee 
	public float fleeXRandMax = 2.0f;			//flee x direction max offset
	public float fleeZRandMax = 2.0f;			//flee y direction max offset
	public float fleeXRandMin = -2.0f;			//flee x direction min offset
	public float fleeZRandMin = -2.0f;			//flee y direction min offset
	[Header("Avoidance Data")]
	public AIAvoidanceStates AIAvoidanceState;	//obstacle avoidance state
	public float avoidMovementTime = 1.0f;		//how long will AI stay in avoidance movement state
	public float lastAvoidanceStanceChangeTime;	//how long since the last avoidance state change
	public float avoidDistance = 3.0f;			//how far ahead will the AI "look" in relation to obstacle avoidance
	public float patrolAvoidDistance = 1.0f;	//how far ahead will the AI "look" in relation to obstacle avoidance while patrolling


	void Awake () {
		target.Clear ();						//clears target array
	}

	// Use this for initialization
	void Start () {
		if (GameManager.gm.twoPlayers) {		//if twoplayer
			i = Random.Range (0, 1);			//set i to random between 0 and 1 for target
		} else {								//if not twoplayer
			i = 0;								//set i to 0 to target first object in target array
		}
		data = GetComponent<TankData> ();			//sets data to tank data
		lastStateChangeTime = Time.time;			//sets last state change time to current time
		lastAvoidanceStanceChangeTime = Time.time;	//sets last avoidance state change to current time
		waypoints = GameManager.gm.AIPatrol;		//pulls waypoints from waypoint list in game manager
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.gm.inGamePlayer != null) {
			target = GameManager.gm.inGamePlayer;									//sets player as AI target
		
			switch (AIPersonality) {												//switch statement for personality types
			case PersonalityType.hunter:									//hunter personality
				switch (AIState) {													//switch statement for AI state
				case AIStates.idle:													//hunter idle state
				//Action
					DoIdle ();														//do idle function
				//Transitions
					if (GameManager.gm.twoPlayers) {
						if (CanSeePlayer (target [0]) || CanSeePlayer (target[1]) || canSeePlayer == true) {			//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Hunter: Tank Seen");			
							ChangeState (AIStates.seekAndShoot);														//change state to seekAndShoot
						} else if (CanSeePlayer (target [0]) || CanSeePlayer (target[1]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Hunter: Tank Heard");
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						} else if (Time.time - lastStateChangeTime > idleWaitTime) {	//if idle wait time has passed
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						}
					} else {
						if (CanSeePlayer (target [0]) || canSeePlayer == true) {			//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Hunter: Tank Seen");			
							ChangeState (AIStates.seekAndShoot);							//change state to seekAndShoot
						} else if (CanHearPlayer (target[0]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Hunter: Tank Heard");
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						} else if (Time.time - lastStateChangeTime > idleWaitTime) {	//if idle wait time has passed
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						}
					}
					break;															//end hunter idle state
				case AIStates.patrol:												//hunter patrol state, should not enter
				//Action
					DoPatrol ();													//do patrol function
				//Transitions
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end hunter patrol state
				case AIStates.shoot:												//hunter shoot state, should not enter
				//Action
					DoShoot ();														//do shoot function
				//Transitions
					if (Time.time - lastStateChangeTime > shootingWaitTime) {		//if shooting wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end hunter shoot state
				case AIStates.patrolAndShoot:										//hunter patrolAndShoot state, should not enter 
				//Action
					DoPatrol ();													//do patrol function
					DoShoot ();														//do shoot function
				//Transitions
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end hunter patrolAndShoot state
				case AIStates.seekPlayer:											//hunter seekPlayer state
				//Action
					DoSeekPlayer (i);												//do seek player function
				//Transitions
					if (GameManager.gm.twoPlayers) {
						if (CanSeePlayer (target[0]) || CanSeePlayer (target[1]) || canSeePlayer == true) {			//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Hunter: Tank Seen");
							ChangeState (AIStates.seekAndShoot);													//change state to seekAndShoot
						}
					} else {
						if (CanSeePlayer (target[0]) || canSeePlayer == true) {			//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Hunter: Tank Seen");
							ChangeState (AIStates.seekAndShoot);						//change state to seekAndShoot
						}
					}
					break;															//end hunter seekPlayer state
				case AIStates.seekAndShoot:											//hunter seekAndShoot state
				//Action
					DoSeekPlayer (i);												//do seek player function
					DoShoot ();														//do shoot function
				//Transitions
					if (GameManager.gm.twoPlayers) {
						if (!CanSeePlayer (target[0]) || !CanSeePlayer (target[1]) || canSeePlayer == false) {			//if CanSeePlayer returns false or canSeePlayer is set to false
							Debug.Log ("Hunter: Tank Lost");
							ChangeState (AIStates.seekPlayer);															//change state to seekPlayer
						}
					} else {
						if (!CanSeePlayer (target[0]) || canSeePlayer == false) {			//if CanSeePlayer returns false or canSeePlayer is set to false
							Debug.Log ("Hunter: Tank Lost");
							ChangeState (AIStates.seekPlayer);								//change state to seekPlayer
						}
					}
					break;															//end hunter seekAndShoot state
				case AIStates.fleePlayer:											//hunter fleePlayer state, should not enter
				//Action
					DoFleePlayer (i);												//do flee player function
				//Transition
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end hunter fleePlayer state
				}
				break;																//end hunter state
			case PersonalityType.defender:									//defender personality
				switch (AIState) {													//switch statement for AI state
				case AIStates.idle:													//defender idle state
				//Action
					DoIdle ();														//do idle function
				//Transitions
					if (GameManager.gm.twoPlayers) {
						if (CanSeePlayer (target[0]) || CanSeePlayer (target[1]) || canSeePlayer == true) {				//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Defender: Tank Seen");
							ChangeState (AIStates.seekAndShoot);														//change state to seekAndShoot
						} else if (CanHearPlayer (target[0]) || CanHearPlayer (target[1]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Defender: Tank Heard");
							ChangeState (AIStates.seekPlayer);															//change state to seekPlayer
						} 
					} else {
						if (CanSeePlayer (target[0]) || canSeePlayer == true) {				//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Defender: Tank Seen");
							ChangeState (AIStates.seekAndShoot);							//change state to seekAndShoot
						} else if (CanHearPlayer (target[0]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Defender: Tank Heard");
							ChangeState (AIStates.seekPlayer);								//change state to seekPlayer
						} 
					}
					break;															//end defender idle state
				case AIStates.patrol:												//defender patrol state
				//Action
					Debug.Log ("Defender: Returning to point");
					DoPatrol ();													//do patrol function
				//Transitions
					if (Time.time - lastStateChangeTime > defenderPatrolTime) {		//if defender patrol time has passed
						Debug.Log ("Defender: Guess I'm here now");
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end defender patrol state
				case AIStates.shoot:												//defender shoot state, should not enter
				//Action
					DoShoot ();														//do shoot function
				//Transitions
					if (Time.time - lastStateChangeTime > shootingWaitTime) {		//if shooting wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end defender shoot state
				case AIStates.patrolAndShoot:										//defender patrolAndShoot state, should not enter 
				//Action
					DoPatrol ();													//do patrol function
					DoShoot ();														//do shoot function
				//Transitions
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end defender patrolAndShoot state
				case AIStates.seekPlayer:											//defender seekPlayer state
				//Action
					DoSeekPlayer (i);												//do seek player function
				//Transitions
					if (GameManager.gm.twoPlayers) {
						if (CanSeePlayer (target[0]) || CanSeePlayer (target[1]) || canSeePlayer == true) {			//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Defender: Tank Seen");
							ChangeState (AIStates.seekAndShoot);													//change state to seekAndShoot
						} else if (Time.time - lastStateChangeTime > defenderHuntTime) {							//if defender hunt time has passed
							ChangeState (AIStates.patrol);															//change state to patrol
						}
					} else {
						if (CanSeePlayer (target[0]) || canSeePlayer == true) {				//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Defender: Tank Seen");
							ChangeState (AIStates.seekAndShoot);							//change state to seekAndShoot
						} else if (Time.time - lastStateChangeTime > defenderHuntTime) {	//if defender hunt time has passed
							ChangeState (AIStates.patrol);									//change state to patrol
						}
					}
					break;															//end defender seekPlayer state
				case AIStates.seekAndShoot:											//defender seekAndShoot state
				//Action
					DoSeekPlayer (i);												//do seek player function
					DoShoot ();														//do shoot function
				//Transitions
					if (Time.time - lastStateChangeTime > defenderHuntTime) {		//if defender hunt time has passed
						ChangeState (AIStates.patrol);								//change state to patrol
					}
					break;															//end defender seekAndShoot state
				case AIStates.fleePlayer:											//defender fleePlayer state, should not enter
				//Action
					DoFleePlayer (i);												//do flee player function
				//Transition				
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end defender fleePlayer state
				}
				break;																//end defender state
			case PersonalityType.fleer:										//fleer personality
				switch (AIState) {													//switch statement for AI state
				case AIStates.idle:													//fleer idle state
				//Action
					DoIdle ();														//do idle function
				//Transitions
					if (GameManager.gm.twoPlayers) {
						if (CanSeePlayer (target[0]) || CanSeePlayer (target[1]) || canSeePlayer == true) {				//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Fleer: Tank Seen");
							ChangeState (AIStates.seekPlayer);															//change state to seekPlayer
						} else if (CanHearPlayer (target[0]) || CanHearPlayer (target[1]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Fleer: Tank Heard");
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						} else if (Time.time - lastStateChangeTime > idleWaitTime) {	//if idle wait time has passed
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						}
					} else {
						if (CanSeePlayer (target[0]) || canSeePlayer == true) {				//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Fleer: Tank Seen");
							ChangeState (AIStates.seekPlayer);								//change state to seekPlayer
						} else if (CanHearPlayer (target[0]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Fleer: Tank Heard");
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						} else if (Time.time - lastStateChangeTime > idleWaitTime) {	//if idle wait time has passed
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						}
					}
					break;															//end fleer idle state
				case AIStates.patrol:												//fleer patrol state, should not enter
				//Action
					DoPatrol ();													//do patrol function
				//Transitions
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end fleer patrol state
				case AIStates.shoot:												//fleer shoot state
				//Action
					DoShoot ();														//do shoot function
				//Transitions
					Debug.Log ("Fleer: Run Away!");
					ChangeState (AIStates.fleePlayer);								//change state to fleePlayer
					break;															//end fleer shoot state
				case AIStates.patrolAndShoot:										//fleer patrolAndShoot state, should not enter 
				//Action
					DoPatrol ();													//do patrol function
					DoShoot ();														//do shoot function
				//Transitions
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end fleer patrolAndShoot state
				case AIStates.seekPlayer:																//fleer seekPlayer state
				//Action
					DoSeekPlayer (i);																	//do seek player function
				//Transitions
					Vector3 playerLocation = GameManager.gm.player[i].transform.position;				//finds player location
					if (Vector3.Distance (transform.position, playerLocation) <= fleerAttackRange) {	//if player tank is within the fleer attack range
						ChangeState (AIStates.shoot);													//change state to shoot
					}
					break;																				//end fleer seekPlayer state
				case AIStates.seekAndShoot:											//fleer seekAndShoot state, should not enter
				//Action
					DoSeekPlayer (i);												//do seek player function
					DoShoot ();														//do shoot function
				//Transitions
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end fleer seekAndShoot state
				case AIStates.fleePlayer:											//fleer fleePlayer state
				//Action
					DoFleePlayer (i);												//do flee player function
				//Transition
					if (Time.time - lastStateChangeTime > fleerFleeTime) {			//if last state change is greater than fleer flee time
						ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
					}
					break;															//end fleer fleePlayer state
				}
				break;																//end fleer state
			case PersonalityType.patroller:									//patroller personality
				switch (AIState) {													//switch statement for AI state
				case AIStates.idle:													//patroller idle state
				//Action	
					DoIdle ();														//do idle function
				//Transitions
					if (GameManager.gm.twoPlayers) {
						if (CanSeePlayer (target[0]) || CanSeePlayer (target[1]) || canSeePlayer == true) {				//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Patroller: Tank Seen");
							ChangeState (AIStates.seekAndShoot);														//change state to seekAndShoot
						} else if (CanHearPlayer (target[0]) || CanHearPlayer (target[1]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Patroller: Tank Heard");
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						} else if (Time.time - lastStateChangeTime > idleWaitTime) {	//if idle wait time has passed
							ChangeState (AIStates.patrol);								//change state to patrol
						}
					} else {
						if (CanSeePlayer (target[0]) || canSeePlayer == true) {				//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Patroller: Tank Seen");
							ChangeState (AIStates.seekAndShoot);							//change state to seekAndShoot
						} else if (CanHearPlayer (target[0]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Patroller: Tank Heard");
							ChangeState (AIStates.seekPlayer);							//change state to seekPlayer
						} else if (Time.time - lastStateChangeTime > idleWaitTime) {	//if idle wait time has passed
							ChangeState (AIStates.patrol);								//change state to patrol
						}
					}
					break;															//end patroller idle state
				case AIStates.patrol:												//fleer patrol state
				//Action
					DoPatrol ();													//do patrol function
				//Transitions
					if (GameManager.gm.twoPlayers) {
						if (CanSeePlayer (target[0]) || CanSeePlayer (target[1]) || canSeePlayer == true) {				//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Patroller: Tank Seen");
							ChangeState (AIStates.seekAndShoot);														//change state to seekAndShoot
						} else if (CanHearPlayer (target[0]) || CanHearPlayer (target[1]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Patroller: Tank Heard");
							ChangeState (AIStates.seekPlayer);															//change state to seekPlayer
						} 
					} else {
						if (CanSeePlayer (target[0]) || canSeePlayer == true) {				//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Patroller: Tank Seen");
							ChangeState (AIStates.seekAndShoot);							//change state to seekAndShoot
						} else if (CanHearPlayer (target[0]) || canHearPlayer == true) {	//if CanHearPlayer returns true or canHearPlayer is set to true
							Debug.Log ("Patroller: Tank Heard");
							ChangeState (AIStates.seekPlayer);								//change state to seekPlayer
						} 
					}
					break;															//end patroller patrol state
				case AIStates.shoot:												//patroller shoot state, should not enter
				//Action
					DoShoot ();														//do shoot function
				//Transitions
					if (Time.time - lastStateChangeTime > shootingWaitTime) {		//if shooting wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end patroller shoot state
				case AIStates.patrolAndShoot:										//patroller patrolAndShoot state, should not enter
				//Action
					DoPatrol ();													//do patrol function
					DoShoot ();														//do shoot function
				//Transitions
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end patroller patrolAndShoot state
				case AIStates.seekPlayer:											//patroller seekPlayer state
				//Action
					DoSeekPlayer (i);												//do seek player function
				//Transitions
					if (GameManager.gm.twoPlayers) {
						if (CanSeePlayer (target[0]) || CanSeePlayer (target[1]) ||  canSeePlayer == true) {			//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Patroller: Tank Seen");
							ChangeState (AIStates.seekAndShoot);														//change state to seekAndShoot
						} else if (Time.time - lastStateChangeTime > waitTime) {										//if wait time has passed
							ChangeState (AIStates.patrol);																//change state to patrol
						}
					} else {
						if (CanSeePlayer (target[0]) || canSeePlayer == true) {			//if CanSeePlayer returns true or canSeePlayer is set to true
							Debug.Log ("Patroller: Tank Seen");
							ChangeState (AIStates.seekAndShoot);						//change state to seekAndShoot
						} else if (Time.time - lastStateChangeTime > waitTime) {		//if wait time has passed
							ChangeState (AIStates.patrol);								//change state to patrol
						}
					}
					break;																//end patroller seekPlayer state
				case AIStates.seekAndShoot:																	//patroller seekAndShoot state
				//Action
					DoSeekPlayer (i);																		//do seek player function
					DoShoot ();																				//do shoot function
				//Transitions
					Vector3 playerLocation = GameManager.gm.player[i].transform.position;					//finds player location
					if (Vector3.Distance (transform.position, playerLocation) >= patrollerAttackRange) {	//if player tank is outside of the patroller attack range
						ChangeState (AIStates.patrol);														//change state to patrol
					} else if (Time.time - lastStateChangeTime > patrollerAttackTime) {						//if patroller attack time has passed
						ChangeState (AIStates.patrol);														//change state to patrol
					}
					break;																					//end patroller seekAndShoot state
				case AIStates.fleePlayer:											//patroller fleePlayer state, should not enter
				//Action
					DoFleePlayer (i);												//do flee player function
				//Transition
					if (Time.time - lastStateChangeTime > waitTime) {				//if wait time has passed
						ChangeState (AIStates.idle);								//change state to idle
					}
					break;															//end patroller fleePlayer state
				}
				break;																//end patroller state
			}
		}
	}

	//end personality area~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	//AI functions

	public void DoIdle () {
		//Do Nothing
	}

	public bool CanSeePlayer (GameObject other) {														//sight function
		Vector3 vectorToTarget;																			//vector
		vectorToTarget = other.transform.position - transform.position;									//sets vectorToTarget to a vector between the game object and another

		float angleToTarget = Vector3.Angle (transform.forward, vectorToTarget);						//finds the angle between the two
		if (angleToTarget <= FOV) {																		//if the object is within range of FOV
			RaycastHit rcData;																			//raycast out data
			if (Physics.Raycast (transform.position, vectorToTarget, out rcData, viewDistance)) {		//raycast forward for range of view distance
				if (rcData.collider.tag == "player" || rcData.collider.tag == "player2") {													//if raycast collider tag is "Tank"
					canSeePlayer = true;																//set canSeePlayer to true
					return true;																		//return true
				}
			}
		}
		canSeePlayer = false;																			//raycast doesnt hit, set canSeePlayer to false
		return false;																					//return false	
	}

	public bool CanHearPlayer (GameObject other) {														//hearing function
		NoiseMaker otherNM = other.GetComponent<NoiseMaker> ();											//communicates with noiseMaker script component
		if (otherNM.isMakingNoise) {																	//if isMakingNoise true (from noiseMaker script)
			if (Vector3.Distance (transform.position, other.transform.position) <= hearingRadius) {		//if target tank is within hearing radius
				canHearPlayer = true;																	//set canHearPlayer to true
				return true;																			//return true
			}
		}
		canHearPlayer = false;																			//player is not making noise or is out of hearing radius, set canHearPlayer to false
		return false;																					//return false
	}
		
	public void ChangeState (AIStates newState) {														//change state function
		lastStateChangeTime = Time.time;																//save time state change						
		AIState = newState;																				//change state
	}

	public void changeAvoidanceState (AIAvoidanceStates newState) {										//obstacle avoidance state change function
		lastAvoidanceStanceChangeTime = Time.time;														//save time state change
		AIAvoidanceState = newState;																	//change state
	}

	public void DoSeekPlayer (int i) {																	//seek player function

		switch (AIAvoidanceState) {																		//obstacle avoidance switch statement
		case AIAvoidanceStates.normal:																	//normal state
			Vector3 playerLocation = GameManager.gm.player[i].GetComponent<Transform> ().position;		//find players location
			Vector3 newDirection = playerLocation - data.tf.position;									//find direction to player
			SendMessage ("RotateTowards", newDirection);												//rotate to player
			SendMessage ("Move", transform.forward * data.forwardSpeed);								//move towards player
			if (!CanMoveForward ()) {																	//raycast forward, if hits something change state
				changeAvoidanceState (AIAvoidanceStates.turnToAvoid);									//change obstacle avoidance state to turnToAvoid
			}
			break;																						//end normal state
		case AIAvoidanceStates.turnToAvoid:																//turnToAvoid state
			Vector3 turnVector = Vector3.zero;															//sets a vector to 0
			turnVector = new Vector3 (0, data.turnSpeed, 0);											//sets vector to turnSpeed
			SendMessage("Rotate", turnVector);															//turn
			if (CanMoveForward ()) {																	//raycast forward, if nothing hit change state
				changeAvoidanceState (AIAvoidanceStates.moveToAvoid);									//change obstacle avoidance state to moveToAvoid
			}
			break;																						//end turnToAvoid state
		case AIAvoidanceStates.moveToAvoid:																//moveToAvoid state
			SendMessage ("Move", transform.forward * data.forwardSpeed);								//move forward
			if (!CanMoveForward ()) {																	//raycast forward, if hit change state
				changeAvoidanceState (AIAvoidanceStates.turnToAvoid);									//change obstacle avoidance state to turnToAvoid
			}
			if (Time.time - lastAvoidanceStanceChangeTime > avoidMovementTime) {						//if time has passed, move to normal
				changeAvoidanceState (AIAvoidanceStates.normal);										//change obstacle avoidance state to normal
			}
			break;																						//end moveToAvoid state
		}

	}
		
	public bool CanMoveForward () {																		//can move forward function
		RaycastHit hitData;																				//raycast forward
		if (Physics.Raycast(data.tf.position, data.tf.forward, out hitData, avoidDistance)) {			//if hit
			if (hitData.collider.tag == "bullet") {														//dont count bullet
				return true;
			}
			if (hitData.collider.tag == "player" || hitData.collider.tag == "player2") {				//dont count tanks
				//ChangeState (AIStates.shoot); 														//may later have state change if tank is seen by enemy
				return true;
			}
			return false;
		} else {																						//otherwise
			return true;																				
		}
	}

	public bool PatrolCanMoveForward () {																//patrol can move forward function
		RaycastHit hitData;																				//raycast forward
		if (Physics.Raycast(data.tf.position, data.tf.forward, out hitData, patrolAvoidDistance)) {		//if hit
			if (hitData.collider.tag == "bullet") {														//dont count bullet
				return true;
			}
			if (hitData.collider.tag == "player" || hitData.collider.tag == "player2") {				//dont count player
				//ChangeState (AIStates.shoot);															//may later have state change if tank is seen by enemy
				return true;
			}
			return false;
		} else {																						//otherwise
			return true;
		}
	}

	public void DoFleePlayer (int i) {																	//do flee player function
		Vector3 playerLocation = GameManager.gm.player[i].transform.position;							//find player location
		Vector3 directionToPlayer = playerLocation - data.tf.position;									//find direction to player
		directionToPlayer = -directionToPlayer;															//reverse it
		directionToPlayer.Normalize();																	//shrink it to 1
		directionToPlayer *= fleeDistance;																//stretch it to how far we wish to flee
		Vector3 pointToFleeTo = data.tf.position + directionToPlayer;									//find where to flee to
		pointToFleeTo.x += Random.Range(fleeXRandMin, fleeXRandMax);									//rando point in x direction
		pointToFleeTo.z += Random.Range(fleeZRandMin, fleeZRandMax);									//rando point in z direction

		switch (AIAvoidanceState) {																		//flee obstacle avoidance switch statement
		case AIAvoidanceStates.normal:																	//normal state
			Vector3 newDirection = pointToFleeTo - data.tf.position;									//seek the point
			SendMessage ("RotateTowards", newDirection);												//turn towards waypoint 
			SendMessage ("Move", transform.forward * data.forwardSpeed);								//move forward
			if (!CanMoveForward ()) {																	//if can move forward function returns false
				changeAvoidanceState (AIAvoidanceStates.turnToAvoid);									//change to turnToAvoid state
			}
			break;																						//end normal state
		case AIAvoidanceStates.turnToAvoid:																//turnToAvoid state
			Vector3 turnVector = Vector3.zero;															//sets a vector to 0
			turnVector = new Vector3 (0, data.turnSpeed, 0);											//sets vector to turnSpeed
			SendMessage("Rotate", turnVector);															//turn
			if (CanMoveForward ()) {																	//raycast forward, if nothing hit change state
				changeAvoidanceState (AIAvoidanceStates.moveToAvoid);									//change obstacle avoidance state to moveToAvoid
			}
			break;																						//end turnToAvoid state
		case AIAvoidanceStates.moveToAvoid:																//moveToAvoid state
			SendMessage ("Move", transform.forward * data.forwardSpeed);								//move forward
			if (!CanMoveForward ()) {																	//raycast forward, if hit change state
				changeAvoidanceState (AIAvoidanceStates.turnToAvoid);									//change obstacle avoidance state to turnToAvoid
			}
			if (Time.time - lastAvoidanceStanceChangeTime > avoidMovementTime) {						//if time has passed, move to normal
				changeAvoidanceState (AIAvoidanceStates.normal);										//change obstacle avoidance state to normal
			}
			break;																						//end moveToAvoid state
		}
	}

	public void DoShoot () {																			//shoot function
		SendMessage ("Shoot", SendMessageOptions.DontRequireReceiver);									//send shoot message
	}

	public void DoPatrol () {																				//patrol function
		currentWaypoint = (int)Mathf.Clamp (currentWaypoint, 0, waypoints.Count-1);							//Keep currentWaypoint in bounds
		switch (AIAvoidanceState) {																			//patrol obstacle avoidance switch statement
		case AIAvoidanceStates.normal:																		//normal state
			Vector3 newDirection = waypoints [currentWaypoint].position - data.tf.position;					//seek the point 
			SendMessage ("RotateTowards", newDirection);													//turn towards waypoint 
			SendMessage ("Move", transform.forward * data.forwardSpeed);									//move forward
			float distanceToWaypoint;																		//proximity variable		
			distanceToWaypoint = Vector3.Distance (data.tf.position, waypoints [currentWaypoint].position);	//find distance to waypoint
			if (distanceToWaypoint <= proximity) {															//if close enough to waypoint
				if (isPatrollingForward) {																	//if patrolling forward
					currentWaypoint++;																		//increase current waypoint count
				} else {																					//if not patrolling forward
					currentWaypoint--;																		//decrease current waypoint count
				}
				if (isPatrollingForward) {																//if we are patrolling forward
					if (currentWaypoint >= waypoints.Count) {											//if we are at the last waypoint
						switch (loopType) {																//patrolling loop switch statement
						case LoopTypes.loop:															//loop state
							currentWaypoint = 0;														//current waypoint is set back to 0
							break;																		//end loop state
						case LoopTypes.random:															//random state
							currentWaypoint = Random.Range (0, waypoints.Count);						//sets a random waypoint from the waypoint list
							break;																		//end random state
						case LoopTypes.pingpong:														//pingpong state (will do to end and reverse path)
							isPatrollingForward = false;												//switch isPatrollingForward to false
							break;																		//end pingpong state
						case LoopTypes.stop:															//stop state
							AIState = AIStates.idle;													//tank will enter the idle state
							break;																		//end stop state
						}
					}
				} else { 																				//when moving backwards. last is item 0
					if (currentWaypoint >= 0) {															//if current waypoint is more then 0
						switch (loopType) {																//patrolling loop switch statement
						case LoopTypes.loop:															//loop state
							currentWaypoint = waypoints.Count - 1;										//current waypoint count is set back by 1
							break;																		//end loop state
						case LoopTypes.random:															//random state
							currentWaypoint = Random.Range (0, waypoints.Count);						//sets a random waypoint from the waypoint list
							break;																		//end random state
						case LoopTypes.pingpong:														//pingpong state (will do to end and reverse path)
							isPatrollingForward = true;													//switch isPatrollingForward to true
							break;																		//end pingpong state
						case LoopTypes.stop:															//stop state
							AIState = AIStates.idle;													//tank will enter the idle state
							break;																		//end stop state
						}
					}

				}
			}
			if (!PatrolCanMoveForward ()) {														//if can move forward function returns false
				changeAvoidanceState (AIAvoidanceStates.turnToAvoid);							//change to turnToAvoid state
			}
			break;																				//end normal state
		case AIAvoidanceStates.turnToAvoid:														//turnToAvoid state
			Vector3 turnVector = Vector3.zero;													//sets a vector to 0
			turnVector = new Vector3 (0, data.turnSpeed, 0);									//sets vector to turnVector
			SendMessage("Rotate", turnVector);													//turn
			if (PatrolCanMoveForward ()) {														//raycast forward, if nothing hit change state
				changeAvoidanceState (AIAvoidanceStates.moveToAvoid);							//change obstacle avoidance state to moveToAvoid
			}
			break;																				//end turnToAvoid state
		case AIAvoidanceStates.moveToAvoid:														//moveToAvoid state
			SendMessage ("Move", transform.forward * data.forwardSpeed);						//move forward
			if (!PatrolCanMoveForward ()) {														//raycast forward, if hit change state
				changeAvoidanceState (AIAvoidanceStates.turnToAvoid);							//change obstacle avoidance state to turnToAvoid
			}
			if (Time.time - lastAvoidanceStanceChangeTime > avoidMovementTime) {				//if time has passed, move to normal
				changeAvoidanceState (AIAvoidanceStates.normal);								//change obstacle avoidance state to normal
			}
			break;																				//end moveToAvoid state
		}
	}
}
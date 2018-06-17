using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	//Public objects
	public static GameManager gm;			//game manager object

	[Header("Character Arrays")]
	public List<PlayerController> player;	//player
	public List<GameObject> AIController;	//ai array
	[Header("Prefabs")]
	public GameObject playerPrefab;			//player prefab
	public GameObject player2Prefab;		//player 2 prefab
	public GameObject hunterPrefab;			//hunter AI prefab
	public GameObject defenderPrefab;		//defender AI prefab
	public GameObject fleerPrefab;			//fleer AI prefab
	public GameObject patrollerPrefab;		//patroller AI prefab
	[Header("Object Lists")]
	public List<Transform> AIPatrol;		//AI patrol list
	public List<GameObject> playerSpawn;	//player spawn list
	public List<GameObject> powerupsList;	//powerup spawn list
	public List<GameObject> roomsInUse;		//rooms in use list
	public List<GameObject> AISpawn;		//AI spawn list
	[Header("Alive Status")]
	public bool playerAlive = false;		//player alive status
	public bool player2Alive = false;		//player 2 alive status
	public int lives = 3;					//player 1 lives
	public int p2Lives = 3;					//player 2 lives
	public bool hunterAlive = false;		//hunter alive status
	public bool defenderAlive = false;		//defender alive status
	public bool fleerAlive = false;			//fleer alive status
	public bool patrollerAlive = false;		//patroller alive status
	//[HideInInspector]
	public List<GameObject> inGamePlayer;	//player object in game
	[Header("Options")]
	public bool twoPlayers;					//twoplayers mode
	public float sound;						//sound volume level
	public float music;						//music volume level
	public GameObject options;				//options game object
	[Header("Scores")]
	public float score1;					//player 1 score
	public float score2;					//player 2 score
	public float hiscore;					//high score
	[Header("Game")]
	public bool gameOver = false;			//game over state
	public bool inGame = false;				//in game state
	[Header("Audio")]
	public GameObject[] gameFx;				//audio sources in game


	//Before any Start
	void Awake() {
		if (gm != null) {						//if game manager is not null
			Destroy (gameObject);				//destroy that object
		} else {								//if game manager is not null
			gm = this;							//set gm object to this
			DontDestroyOnLoad (gameObject);		//dont destroy this object on scene load
		}
		if (options == null) {												//if options is null
			options = GameObject.FindGameObjectWithTag ("destroyOptions");	//find options gameobject
		}
		player.Clear ();													//clear player array
		inGamePlayer.Clear ();												//clear inGamePlayer array

	}

	// Use this for initialization
	void Start () {
		hiscore = PlayerPrefs.GetFloat ("hiscore");					//load highscore from player prefs
		twoPlayers = OptionsSettings.os.twoPlayer;					//pull two player stat from options
		sound = OptionsSettings.os.soundLevel;						//pull sound level stat from options
		music = OptionsSettings.os.musicLevel;						//pull music level stat from options
		Destroy (options);											//destroy options 
		inGame = true;												//set inGame state to true
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!playerAlive && lives > 0 && !gameOver) {							//if player is not currently alive and not game over
			SpawnPlayer ();														//spawn the player
		}
		if (!player2Alive && twoPlayers && p2Lives > 0 && !gameOver) {			//if twoplayer and player 2 is not alive and not game over
			SpawnPlayer2 ();													//spawn player 2
		}
		if (playerAlive && !hunterAlive && !gameOver) {							//is player is currently alive and hunter is not and not game over
			SpawnHunter ();														//spawn hunter AI
		}
		if (playerAlive && !defenderAlive && !gameOver) {						//is player is currently alive and defender is not and not game over
			SpawnDefender ();													//spawn defender AI
		}
		if (playerAlive && !fleerAlive && !gameOver) {							//is player is currently alive and fleer is not and not game over
			SpawnFleer ();														//spawn fleer AI
		}
		if (playerAlive && !patrollerAlive && !gameOver) {						//is player is currently alive and patroller is not and not game over
			SpawnPatroller ();													//spawn paroller AI
		}

		if (!twoPlayers) {														//if not two player mode
			if (lives == 0) {													//and lives is 0
				killAll ();														//run kill function
				gameOver = true;												//game over state is true
				SceneManager.LoadScene ("End");									//load end scene
				lives = -1;														//set lives to anything but 0
			}
		} else if (twoPlayers) {												//if two player mode
			if (lives == 0 && p2Lives == 0) {									//and both lives is 0
				killAll ();														//run kill function
				gameOver = true;												//game over state is true
				SceneManager.LoadScene ("End");									//load end scene
				lives = -1;														//set lives to anything but 0
				p2Lives = -1;													//set player 2 lives to anything but 0
			}
		}

		scoreKeeping ();														//run score keeping function

		if (inGame) {															//if in game state is true
			gameFx = GameObject.FindGameObjectsWithTag ("TankAudio");			//find all objects with TankAudio tag and add to gameFx array
			for (int i = 0; i < gameFx.Length; i++) {							//for each object in gameFx array
				AudioSource temp = gameFx [i].GetComponent<AudioSource> ();		//create temp audio source object 
				if (temp != null) {												//if temp is not null
					temp.volume = sound;										//assign volume level to sound
				}
			}
		}
	}

	public void killAll() {														//kill function
		if (AIController.Count > 0) {											//until all AI tanks are destroyed
			foreach (GameObject go in AIController) {							//each object in array
				Destroy (go);													//destroy game object
			}
		}
	}

	public void scoreKeeping() {												//score keeping function
		if (!twoPlayers) {														//if not two player
			if (hiscore < score1) {												//if score is higher than hiscore
				hiscore = score1;												//set hiscore to score
			}
		} else if (twoPlayers) {												//if two player
			if (score1 > score2) {												//if score 1 is higher than score 2
				if (hiscore < score1) {											//if score 1 is higher than hiscore
					hiscore = score1;											//set hiscore to score 1
				}
			} else {															//if score 1 is not higher than score 2
				if (hiscore < score2) {											//if score 2 is higher than hiscore
					hiscore = score2;											//set hiscore to score 2
				}
			}
		}
		if (gameOver) {															//if game over state is true
			PlayerPrefs.SetFloat ("hiscore", hiscore);							//set hiscore to hiscore in player prefs
			PlayerPrefs.Save ();												//save player prefs
		}
	}

	public void SpawnPlayer() {																										//player character spawner
		int spawnPoints = Random.Range (0, playerSpawn.Count);																		//pick a random spawn from the playerSpawn list
		Instantiate (playerPrefab, playerSpawn[spawnPoints].transform.position, playerSpawn[spawnPoints].transform.rotation);		//spawn player at chosen spawn point
		playerAlive = true;																											//set playerAlive in game manager
	}

	public void SpawnPlayer2() {
		int spawnPoints = Random.Range (5, playerSpawn.Count);																		//pick a random spawn from the playerSpawn list
		Instantiate (player2Prefab, playerSpawn[spawnPoints].transform.position, playerSpawn[spawnPoints].transform.rotation);		//spawn player at chosen spawn point
		player2Alive = true;																										//set player2Alive in game manager
	}

	public void SpawnHunter() {																										//hunter AI spawner
		int AIPoints = Random.Range (0, AISpawn.Count);																				//pick a random spawn from AISpawn list
		Instantiate (hunterPrefab, AISpawn[AIPoints].transform.position, AISpawn[AIPoints].transform.rotation);						//spawn hunter at chosen point
		hunterAlive = true;																											//set hunterAlive in game manager
	}

	public void SpawnDefender() {																									//defender AI spawner
		int AIPoints = Random.Range (4, AISpawn.Count - 1);																			//pick a random spawn from AISpawn list between 4 and one less than the count of the list
		Instantiate (defenderPrefab, AISpawn [AIPoints].transform.position, AISpawn [AIPoints].transform.rotation);					//spawn defender at chosen point
		defenderAlive = true;																										//set defenderAlive in game manager
	}

	public void SpawnFleer() {																										//fleer AI spawner
		int AIPoints = Random.Range (3, AISpawn.Count - 2);																			//pick a random spawn from AISpawn list between 3 and two less than the count of the list
		Instantiate (fleerPrefab, AISpawn [AIPoints].transform.position, AISpawn [AIPoints].transform.rotation);					//spawn fleer at chosen point
		fleerAlive = true;																											//set fleerAlive in game manager
	}

	public void SpawnPatroller() {																									//patroller AI spawner
		int AIPoints = Random.Range (0, AISpawn.Count);																				//pick a random spawn from AISpawn list
		Instantiate (patrollerPrefab, AISpawn [AIPoints].transform.position, AISpawn [AIPoints].transform.rotation);				//spawn patroller at chosen point
		patrollerAlive = true;																										//set patrollerAlive in game manager
	}
}
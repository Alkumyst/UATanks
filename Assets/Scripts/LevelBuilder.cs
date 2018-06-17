using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

	public int cols;							//columns to build
	public int rows;							//rows to build
	public Room [,] grid;						//environment grid
	public float tileWidth = 50;				//tile width
	public float tileLength = 50;				//tile length
	public List<GameObject> roomPrefabs;		//room prefab list to spawn from
	public bool useSeed;						//use seed when building level
	public int levelSeed;						//level seed
	private System.DateTime today;				//today's date
	private int mYear;							//year
	private int mDay;							//day
	private int mMonth;							//month
	private int daySeed;						//level of the day seed
	public bool levelOfDay;						//use level of the day seed for level seed
	public bool gameReady = false;				//game ready wait
	public GameObject gm;						//gameManager object
	private int levelGen = 0;					//level generation lock
	public static LevelBuilder lb;				//level generation object

	// Use this for initialization
	void Awake () {

		if (lb != null) {						//if lb is not null
			Destroy (gameObject);				//destroy that object
		} else {								//if lb is not null
			lb = this;							//set lb object to this
		}

		today = System.DateTime.Now;						//set today to today's date
		mYear = today.Year;									//pull the year from today
		mDay = today.Day;									//pull the day from today
		mMonth = today.Month;								//pull the month from today
		daySeed = mYear * 10000 + mDay + mMonth * 100;		//set daySeed to YYYYDDMM format



	}

	void seedChoice() {
		if (levelOfDay) {									//if using level of the day seed
			Random.InitState (daySeed);						//set random initial state to daySeed
		} else if (useSeed) {								//if using a seed
			Random.InitState (levelSeed);					//set random initial state to levelSeed
		} else {
			//do nothing
		}
	}

	// Update is called once per frame
	void Update () {

		if (gameReady == true && levelGen == 0){				//if gameReady and level gen is 0
			seedChoice ();										//run seed choice function
			GenerateGrid ();									//generate the level
			gm.SetActive (true);								//awake gm
			levelGen++;											//lock level gen
		}
	}

	public void GenerateGrid () {
		grid = new Room[cols,rows];																				//setup grid array
		for (int currentRow = 0; currentRow < rows; currentRow++) {												//once for each row
			for (int currentCol = 0; currentCol < cols; currentCol++) {											//then once for each column
				Vector3 position = new Vector3 (currentCol * tileWidth, 0, currentRow * tileLength);			//set position based on tile width and length
				GameObject prefabToSpawn = roomPrefabs [Random.Range (0, roomPrefabs.Count)];					//pick room to spawn
				GameObject temp = Instantiate (prefabToSpawn, position, Quaternion.identity) as GameObject;		//spawn room
				temp.name = "Tile "+currentCol+","+currentRow;													//name it
				temp.transform.parent = transform.parent;														//set parent
				Room tempRoom = temp.GetComponent<Room> ();														//open doors
				if (currentCol != 0) {
					tempRoom.doorWest.SetActive (false);
				}
				if (currentCol != cols - 1) {
					tempRoom.doorEast.SetActive (false);
				}
				if (currentRow != 0) {
					tempRoom.doorSouth.SetActive (false);
				}
				if (currentRow != rows - 1) {
					tempRoom.doorNorth.SetActive (false);
				}
				grid [currentCol, currentRow] = tempRoom;														//store it in grid

			}
		}
	}
}

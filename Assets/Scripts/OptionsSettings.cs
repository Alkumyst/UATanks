using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsSettings : MonoBehaviour {

	[Header("Options")]
	public bool twoPlayer;
	public float soundLevel;
	public float musicLevel;
	[Header("Objects")]
	public Slider soundSlider;
	public Slider musicSlider;
	public Toggle twoPlayerTog;
	public AudioSource menuMusic;
	public AudioSource gameMusic;

	private int twoPlayerStat;
	public bool inGame;

	public static OptionsSettings os;

	void Awake () {
		DontDestroyOnLoad (gameObject);
		if (os != null) {						//if os is not null
			Destroy (gameObject);				//destroy that object
		} else {								//if os is not null
			os = this;							//set os object to this
		}
	}

	// Use this for initialization
	void Start () {
		twoPlayerStat = PlayerPrefs.GetInt ("twoPlayer");		//load twoplayer status from player prefs
		if (twoPlayerStat == 1) {								//if twoPlayerStat is 1
			twoPlayerTog.isOn = true;							//set toggle to true
		} else {												//else
			twoPlayerTog.isOn = false;							//set toggle to false
		}
		soundLevel = PlayerPrefs.GetFloat ("soundOption");		//load soundLevel from player prefs
		soundSlider.value = soundLevel;							//set slider value to sound level
		musicLevel = PlayerPrefs.GetFloat ("musicOption");		//load musicLevel from player prefs
		musicSlider.value = musicLevel;							//set slider value to music level
	}
	
	// Update is called once per frame
	void Update () {
		soundLevel = soundSlider.value;							//set sound level to slider value
		musicLevel = musicSlider.value;							//set music level to slider value

		if (twoPlayerStat == 0) {								//if twoPlayerStat is 0
			twoPlayer = false;									//two player is false
		} else if (twoPlayerStat == 1) {						//if twoPlayerStat is 1
			twoPlayer = true;									//two player is true
		}
		if (menuMusic != null) {								//if menuMusic is no null
			menuMusic.volume = musicLevel;						//set volume from musicLevel
		}
		if (inGame) {																					//if in game
			gameMusic = GameObject.FindGameObjectWithTag ("GameMusic").GetComponent<AudioSource> ();	//find in game music object
			if (gameMusic != null) {																	//if in game music is not null
				gameMusic.volume = musicLevel;															//set volume from musicLevel
			}
		}
	}

	public void saveOptions() {	
		PlayerPrefs.SetInt ("twoPlayer", twoPlayerStat);		//set twoPlayerStat to twoPlayer in player prefs
		PlayerPrefs.SetFloat ("soundOption", soundLevel);		//set soundLevel to soundOption in player prefs
		PlayerPrefs.SetFloat ("musicOption", musicLevel);		//set musicLevel to soundOption in player prefs
		PlayerPrefs.Save ();									//save player prefs
	}

	public void TwoPlayer () {									
		if (twoPlayer) {										//if twoPlayer
			twoPlayer = false;									//change twoPlayer to false
			twoPlayerStat = 0;									//set stat to 0
		} else if (!twoPlayer) {								//else
			twoPlayer = true;									//change twoPlayer to true
			twoPlayerStat = 1;									//set stat to 1
		}
	}
}

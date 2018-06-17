using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfo : MonoBehaviour {

	public Text lives;				//lives text box
	public Text score;				//score text box
	public Text health;				//health text box

	private float healthF;			//health float
	private string healthS;			//health string
	private float scoreF;			//score float
	private string scoreS;			//score string
	private float livesF;			//lives float
	private string livesS;			//lives string

	void healthUpdate (float h) {		//health update function
		healthF = h;					//assign health float to input argument
		healthS = healthF.ToString ();	//convert float to string
		health.text = healthS;			//assign new string to text box
	}

	void scoreUpdate (float s) {		//score update function
		scoreF = s;						//assign score float to input argument
		scoreS = scoreF.ToString ();	//convert float to string
		score.text = scoreS;			//assign new string to text box
	}

	void livesUpdate(float l) {			//lives update function
		livesF = l;						//assign lives float to input argument
		livesS = livesF.ToString ();	//convert float to string
		lives.text = livesS;			//assign new string to text box
	}
}

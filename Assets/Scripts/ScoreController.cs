using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	public Text finalP1Score;
	public Text finalP2Score;
	public Text hiScore;

	public GameObject p2Score;

	private float p1ScoreF;
	private string p1ScoreS;
	private float p2ScoreF;
	private string p2ScoreS;
	private float hiScoreF;
	private string hiScoreS;

	void Awake () {
		p1ScoreF = GameManager.gm.score1;			//pull player1 score from game manager
		p1ScoreS = p1ScoreF.ToString ();			//convert player1 score float to string
		if (GameManager.gm.twoPlayers == true) {	//if twoPlayer mode
			p2ScoreF = GameManager.gm.score2;		//pull player2 score from game manager
			p2ScoreS = p2ScoreF.ToString ();		//convert player2 score float to string
		}
		hiScoreF = GameManager.gm.hiscore;			//pull hiscore from game manager
		hiScoreS = hiScoreF.ToString ();			//convert hiscore float to string
	}

	// Use this for initialization
	void Start () {

		if (GameManager.gm.twoPlayers == false) {	//if not twoPlayer
			p2Score.SetActive (false);				//turn off player 2 score
			finalP1Score.text = p1ScoreS;			//final score text box for player 1 set from player 1 score string
			hiScore.text = hiScoreS;				//hiscore text box set from hiscore string
		} else {									//else (if twoPlayer)
			finalP1Score.text = p1ScoreS;			//final score text box for player 1 set from player 1 score string
			finalP2Score.text = p2ScoreS;			//final score text box for player 2 set from player 2 score string
			hiScore.text = hiScoreS;				//hiscore text box set from hiscore string
		}
	}
}

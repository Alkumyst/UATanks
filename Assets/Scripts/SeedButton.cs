using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedButton : MonoBehaviour {

	public GameObject seedCam;
	public GameObject canvas;
	public GameObject eventSys;
	public GameObject rButton;
	public GameObject cButton;
	public GameObject lotdButton;
	public GameObject seedInput;
	public GameObject confirmButton;
	public GameObject backButton;
	public InputField textbox;
	public GameObject displayTank;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void randomSeed() {
		LevelBuilder.lb.useSeed = false;		//use seed set to false
		LevelBuilder.lb.levelOfDay = false;		//use level of day seed set to false
		Destroy(seedCam);						//destroy seed cam
		canvas.SetActive (false);				//turn off canvas
		eventSys.SetActive (false);				//turn off event system
		displayTank.SetActive (false);			//turn off display tank
		LevelBuilder.lb.gameReady = true;		//tell level builder game is ready

	}

	public void customSeed() {
		LevelBuilder.lb.useSeed = true;			//use seed set to true
		LevelBuilder.lb.levelOfDay = false;		//use level of day seed set to false
		rButton.SetActive (false);				//turn off button
		cButton.SetActive (false);				//turn off button
		lotdButton.SetActive (false);			//turn off button
		seedInput.SetActive (true);				//turn on text input
		confirmButton.SetActive (true);			//turn on button
		backButton.SetActive (true);			//turn on button
		LevelBuilder.lb.gameReady = false;		//tell level builder game is still not ready
	}

	public void BackButton() {
		LevelBuilder.lb.useSeed = false;		//use seed set to false
		LevelBuilder.lb.levelOfDay = false;		//use level of day seed set to false
		rButton.SetActive (true);				//turn on button
		cButton.SetActive (true);				//turn on button
		lotdButton.SetActive (true);			//turn on button
		seedInput.SetActive (false);			//turn off text input
		confirmButton.SetActive (false);		//turn off button
		backButton.SetActive (false);			//turn off button
		LevelBuilder.lb.gameReady = false;		//tell level builder game is still not ready
	}

	public void lotdSeed() {
		LevelBuilder.lb.useSeed = false;		//use seed set to false
		LevelBuilder.lb.levelOfDay = true;		//use level of day seed set to true
		Destroy(seedCam);						//destroy seed cam
		canvas.SetActive (false);				//turn off canvas
		eventSys.SetActive (false);				//turn off event system
		displayTank.SetActive (false);			//turn off display tank
		LevelBuilder.lb.gameReady = true;		//tell level builder game is ready
	}	

	public void comfirmSeed() {
		LevelBuilder.lb.levelSeed = int.Parse (textbox.text);	//pull data from text input and assign it to level seed in level builder
		Destroy(seedCam);										//destroy seed cam
		canvas.SetActive (false);								//turn off canvas
		eventSys.SetActive (false);								//turn off event system
		displayTank.SetActive (false);							//turn off display tank
		LevelBuilder.lb.gameReady = true;						//tell level builder game is ready
	}
}

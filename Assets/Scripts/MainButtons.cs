using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtons : MonoBehaviour {

	public GameObject startButton;
	public GameObject optionsButton;
	public GameObject endButton;
	public GameObject returnButton;
	public GameObject musicSlider;
	public GameObject soundSlider;
	public GameObject twoPlayerToggle;

	public OptionsSettings optSet;

	public void StartGame() {
		SceneManager.LoadScene ("Main");		//load main scene
	}

	public void EndGame() {			
		Application.Quit ();					//exit application
	}

	public void OptionsButton() {
		startButton.SetActive (false);			//turn off button
		optionsButton.SetActive (false);		//turn off button
		endButton.SetActive (false);			//turn off button
		returnButton.SetActive (true);			//turn on button
		musicSlider.SetActive (true);			//turn on slider
		soundSlider.SetActive (true);			//turn on slider
		twoPlayerToggle.SetActive (true);		//turn on toggle
	}

	public void ReturnButton() {
		startButton.SetActive (true);			//turn on button
		optionsButton.SetActive (true);			//turn on button
		endButton.SetActive (true);				//turn on button
		returnButton.SetActive (false);			//turn off button
		musicSlider.SetActive (false);			//turn off slider
		soundSlider.SetActive (false);			//turn off slider
		twoPlayerToggle.SetActive (false);		//turn off toggle
		optSet.saveOptions ();					//run saveOptions function
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour {
	public Text scoreText;
	public GameObject newGamePanel;

	// Use this for initialization
	void Start () {
		newGamePanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.GetComponent<Text> ().text = GameObject.Find ("GameManager").GetComponent<GameManagerScript> ().score.ToString ();
	}
	public void newGameBt(){
		newGamePanel.SetActive(true);
	}
	public void EM(){
		SceneManager.LoadScene ("ExerciseMode");
	}
	public void YM(){
		SceneManager.LoadScene ("YogaMode");
	}
	public void exit(){
		GameObject.Find ("GameManager").GetComponent<GameManagerScript> ().TurnOff();
		Application.Quit ();
	}
}

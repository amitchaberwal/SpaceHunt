using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EMUI : MonoBehaviour {
	public Text scoreText;
	public Text angleText;
	public Text AltitudeText;
	public Text BpText;
	public Text ThresholdText;
	public Slider health;

	public Text dValue;
	public Slider mSlider;
	public GameObject MenuPanel;
	void Start () {
		MenuPanel.SetActive (false);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.GetComponent<Text>().text = GameObject.Find ("ufo").GetComponent<ufo> ().score.ToString();
		angleText.GetComponent<Text>().text = GameObject.Find ("ufo").GetComponent<ufo> ().angle.ToString();
		AltitudeText.GetComponent<Text>().text = GameObject.Find ("ufo").GetComponent<ufo> ().altitude.ToString();
		health.GetComponent<Slider>().value = GameObject.Find ("ufo").GetComponent<ufo> ().health;
		BpText.GetComponent<Text>().text = GameManagerScript.recievedbp.ToString();
		ThresholdText.GetComponent<Text>().text = ufo.thresh.ToString();
		mSlider.GetComponent<Slider> ().value = GameManagerScript.difficulty;
		dValue.GetComponent<Text> ().text = mSlider.GetComponent<Slider> ().value.ToString();
		
	}
	public void pausebt(){
		MenuPanel.SetActive (true);
	}
	public void mainMenu(){
		Destroy (GameObject.Find ("GameManager"));
		SceneManager.LoadScene ("UI Scene");
	}
	public void exitbt(){
		GameObject.Find ("GameManager").GetComponent<GameManagerScript> ().TurnOff();
		Application.Quit ();
	}
	public void backbt(){
		MenuPanel.SetActive (false);
	}
	public void dfslider(){
		GameManagerScript.difficulty = (int)mSlider.GetComponent<Slider>().value;
	}
}

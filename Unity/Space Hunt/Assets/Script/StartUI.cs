using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class StartUI : MonoBehaviour {
	public GameObject newgameCanvas;
	public GameObject mainCanvas;
	public GameObject settingCanvas;
	public GameObject connectCanvas;
	public GameObject difficultyCanvas;
	public GameObject AUPanel;
	public  Text  messageText;
	public Slider difficultySlider;
	public Text df;
	public Dropdown pairedDevice;
	public Text inputText;
	public Text bpm;
	public Text thresh;

	string[] deviceName;
	public static int sendname;
	bool search = true;
	// Use this for initialization
	void Start () {
		sendname  = 0;
		newgameCanvas.SetActive(false);
		settingCanvas.SetActive (false);
		connectCanvas.SetActive (false);
		difficultyCanvas.SetActive (false);
		mainCanvas.SetActive (true);
		AUPanel.SetActive (false);
		messageText.GetComponent<Text>().text = "Null";
		
	}
	public void APlugin(string smsg){
		messageText.GetComponent<Text>().text = smsg;
	}
	public void APluginVal(int sms){
		inputText.GetComponent<Text>().text = sms.ToString();
	}
	public void AddDevices(string foundNames)
	{
		if (search == true) {
			foundNames = foundNames.Trim ('[', ']');
			deviceName = foundNames.Split (',');
			List<string> names = new List<string> (deviceName);
			pairedDevice.AddOptions (names);
			search = false;
		}
	}

	public void dfLevel(){
		GameManagerScript.difficulty = (int)difficultySlider.GetComponent<Slider>().value;
	}
	public void dpselect(int indx){
		sendname = indx;
	}

	// Update is called once per frame
	void Update () {
		if (GameManagerScript.recievedbp == 0) {
			bpm.GetComponent<Text> ().text = "Not Connected";
			thresh.GetComponent<Text> ().text = "Not Connected";
		} else {
			bpm.GetComponent<Text>().text = GameManagerScript.recievedbp.ToString();
			thresh.GetComponent<Text>().text = GameManagerScript.threshold.ToString();
		}
		df.GetComponent<Text> ().text = GameManagerScript.difficulty.ToString ();
		inputText.GetComponent<Text>().text = GameManagerScript.recievedbp.ToString();
	}

	public void Settingbt() {
		mainCanvas.SetActive (false);
		settingCanvas.SetActive (true);
		connectCanvas.SetActive (false);
		difficultyCanvas.SetActive (false);
	}
	public void backbt(){
		newgameCanvas.SetActive(false);
		settingCanvas.SetActive (false);
		mainCanvas.SetActive (true);
		connectCanvas.SetActive (false);
		difficultyCanvas.SetActive (false);
	}
	public void newGamebt(){
		newgameCanvas.SetActive(true);
	}
	public void connectbt(){
		GameObject.Find ("GameManager").GetComponent<GameManagerScript> ().TurnOn();
		connectCanvas.SetActive (true);
		difficultyCanvas.SetActive (false);

	}
	public void difficultybt(){
		connectCanvas.SetActive (false);
		difficultyCanvas.SetActive (true);

	}
		
	public void exit(){
		GameObject.Find ("GameManager").GetComponent<GameManagerScript> ().TurnOff();
		Application.Quit ();

	}
	public void AUButton(){
		AUPanel.SetActive (true);
	}
	public void AUBackBt(){
		AUPanel.SetActive (false);
	}
}

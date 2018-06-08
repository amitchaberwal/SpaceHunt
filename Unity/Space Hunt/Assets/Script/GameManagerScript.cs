using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {
	AndroidJavaClass ajc;
	AndroidJavaObject ajo;

	public int score = 0;
	public static int recievedbp = 0;
	public static float threshold = 0;
	public static int difficulty = 1;

	public static int[] readings = new int[5];
	int i = 0;
	int recieved = 0;
	int[] outread = new int[4];

	public int[] thresh = new int[4] ;
	public int threshIndex = 0;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
	void Update () {

	}

	public void Plugin(string smsg){
		GameObject.Find ("UIManager").GetComponent<StartUI> ().APlugin (smsg);
	}
	public void BPM(string msg)
	{
		if (msg != null) {
			recieved = int.Parse (msg);
			recievedbp = recieved;
			if (threshIndex <= 3 && recievedbp != 0) {
				addHR (recievedbp);
			} else {
				threshold = getThreshold();
			}
		}
	}
	public void TurnOn()
	{
		AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ajo = new AndroidJavaObject ("com.v4tech.spacehunt.Bluetooth");
		ajo.Call ("TurnOn");
	}
	public void GetDevices(){
		AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ajo = new AndroidJavaObject ("com.v4tech.spacehunt.Bluetooth");
		ajo.Call("GetDevices");
	}
	public void DeviceList(string foundNames)
	{
		GameObject.Find ("UIManager").GetComponent<StartUI> ().AddDevices (foundNames);
	}
	public void GetData()
	{
		AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ajo = new AndroidJavaObject ("com.v4tech.spacehunt.Bluetooth");
		GameObject.Find ("UIManager").GetComponent<StartUI> ().APlugin ("Connecting");
		ajo.Call ("GetData",StartUI.sendname.ToString());
		ajo.Call ("sendSignal","AF");
	}

	public void TurnOff(){
		AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ajo = new AndroidJavaObject ("com.v4tech.spacehunt.Bluetooth");
		//ajo.Call ("sendSignal","S");
		ajo.Call ("TurnOff");
	}


	public void startEM(){
		SceneManager.LoadScene ("ExerciseMode",LoadSceneMode.Single);

	}
	public void startYM(){
		SceneManager.LoadScene ("YogaMode",LoadSceneMode.Single);
	}


	//Threshold
	public void addHR(int heartrate){
		thresh [threshIndex] = heartrate;
		threshIndex += 1;
	}
	public int getThreshold(){
		float sum = 0;
		for (int i = 0; i <= 3; i++) {
			sum += thresh [i];
		}
		return Mathf.RoundToInt (sum / 4);
	}
}

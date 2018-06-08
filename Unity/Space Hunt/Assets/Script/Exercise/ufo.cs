using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ufo : MonoBehaviour {
	public float angle;
	public float altitude;
	float pitch = 1.0f;
	public AudioSource sound;
	public int score = 0;
	public float health = 100;

	public static float ufoAngle = 0;
	public static int bp;
	public static float thresh;
	public int diff = 1;
	void Start () {
		
	}

	void Update () {
		bp = GameManagerScript.recievedbp;
		thresh = GameManagerScript.threshold + 5;
		diff = GameManagerScript.difficulty;
		ufoAngle = bp - thresh;
		if (ufoAngle >= 90) {
			ufoAngle = 90;
		}
		if (ufoAngle <= -90) {
			ufoAngle = -90;
		}
		if (ufoAngle < 0) {
			ufoAngle = ufoAngle * diff;
		}
		if (ufoAngle >= 0) {
			ufoAngle = ufoAngle / diff;
		}
		angle = ufoAngle;
		altitude = gameObject.transform.position.y;
		altitude = altitude + (Mathf.Sin (angle * Mathf.Deg2Rad))/20;
		if (altitude >= 4.4f) {
			altitude = 4.4f;
		}
		if (altitude <= -4.4f) {
			altitude = -4.4f;
		}
		pitch = 1.0f + angle / 180;
		sound.pitch = pitch;
		gameObject.transform.position = new Vector3 (-2.96f, altitude, 0);
		gameObject.transform.eulerAngles = new Vector3 (0, 0, angle);
		if (gameObject.transform.position.y <= -4.0f) {
			health -= Time.deltaTime * 5f;
		}

	}
}


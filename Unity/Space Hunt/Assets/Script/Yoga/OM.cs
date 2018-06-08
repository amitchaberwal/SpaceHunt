using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OM : MonoBehaviour {
	public float alienSpeed = 1f;
	private GameObject playerTriggered;

	// Use this for initialization
	void Start () {
		playerTriggered = GameObject.Find("player");
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position -= new Vector3 (alienSpeed*Time.deltaTime, 0, 0);
		if (gameObject.transform.position.x <= -9f) {
			Destroy (gameObject);
		}
		
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Player") {
			playerTriggered.GetComponent<YPlayer> ().score += 1;
			Destroy (gameObject);
		}

	}
}

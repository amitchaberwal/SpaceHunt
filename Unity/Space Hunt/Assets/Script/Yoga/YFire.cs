using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YFire : MonoBehaviour {
	public float speedX = 0;
	float angle;

	// Use this for initialization
	void Start () {
		
	}

	void Update () {
		angle = ExcerciseMode.ufoAngle;
		speedX = 0.5f -  Mathf.Abs(angle / 180);
		MeshRenderer mr = GetComponent<MeshRenderer> ();
		Material mat = mr.material;
		Vector2 offset = mat.mainTextureOffset;
		offset.x -= Time.deltaTime * speedX;
		mat.mainTextureOffset = offset;
		
	}
}

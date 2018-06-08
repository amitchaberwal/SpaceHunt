using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollSpace : MonoBehaviour {
	float speedX = 0;
	float speedy = 0;
	public float angle;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		angle = ExcerciseMode.ufoAngle;
		speedX = 0.5f -  Mathf.Abs(angle / 180);
		speedy = angle / 150;
		MeshRenderer mr = GetComponent<MeshRenderer> ();
		Material mat = mr.material;
		Vector2 offset = mat.mainTextureOffset;
		offset.x += Time.deltaTime * speedX;
		offset.y += Time.deltaTime * speedy;
		mat.mainTextureOffset = offset;
		
	}
}

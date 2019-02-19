using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBladeMovement : MonoBehaviour {
    public float moveSpeed;
	// Use this for initialization
	void Start () {
        if (moveSpeed <= 0)
            moveSpeed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
	}
}

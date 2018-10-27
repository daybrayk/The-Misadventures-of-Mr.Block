using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool isMoving;
    public Vector2 ropeHook;
    public float swingForce;
	// Use this for initialization
	void Start () {
        if (swingForce <= 0)
            swingForce = 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

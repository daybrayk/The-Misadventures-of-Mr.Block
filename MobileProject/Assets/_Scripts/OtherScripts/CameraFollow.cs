﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player;
	// Use this for initialization
	void Start () {
		
	}

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x + 1.95f, 0, -10);
    }
}

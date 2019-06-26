using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofMove : MonoBehaviour {
    public static GameObject frontTile;
    public Transform camera;
	// Use this for initialization
	void Start () {
        frontTile = GameObject.FindWithTag("FrontTile");
	}
	
	// Update is called once per frame
	void Update () {
        if (camera.position.x - transform.position.x > 9.8f)
        {
            transform.position = new Vector2(frontTile.transform.position.x + 1.25f, transform.position.y);
            frontTile = gameObject;
        }
    }
}

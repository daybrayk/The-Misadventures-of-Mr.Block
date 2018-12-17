using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofMove : MonoBehaviour {
    public static GameObject frontTile;
	// Use this for initialization
	void Start () {
        frontTile = GameObject.FindWithTag("FrontTile");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GarbageCollector")
        {
            transform.position = new Vector2(frontTile.transform.position.x + 1.27f, frontTile.transform.position.y);
            frontTile = gameObject;
        }
    }
}

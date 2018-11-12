using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMove : MonoBehaviour {
    float moveSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float y = transform.position.y + (moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, y, 0);
	}

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "GarbageCollector")
            Destroy(gameObject);
    }
}

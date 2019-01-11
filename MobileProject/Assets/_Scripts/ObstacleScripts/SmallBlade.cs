using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBlade : MonoBehaviour {
    public float rotationalVel;
    public float moveSpeed;

	// Use this for initialization
	void Start () {
        if (moveSpeed <= 0)
            moveSpeed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + (rotationalVel * Time.deltaTime));
        transform.position = new Vector3(transform.position.x - (moveSpeed * Time.deltaTime), transform.position.y, transform.position.z);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject g = collision.gameObject;
        if(g.tag == "Player")
        {
            g.GetComponent<PlayerController>().Slice();
        }
    }
}

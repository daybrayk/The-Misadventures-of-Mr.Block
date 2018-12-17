using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBlade : MonoBehaviour {
    public Transform t;
    public float rotationalVel;
    public float moveSpeed;

	// Use this for initialization
	void Start () {
        t = GetComponent<Transform>();
        if (moveSpeed <= 0)
            moveSpeed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
        t.eulerAngles = new Vector3(0, 0, t.eulerAngles.z + (rotationalVel * Time.deltaTime));
        t.position = new Vector3(t.position.x - (moveSpeed * Time.deltaTime), t.position.y, t.position.z);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBlade : MonoBehaviour {
    public float rotationalVel;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + (rotationalVel * Time.deltaTime));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlade : MonoBehaviour {
    public Transform t;
    public float rotationalVel;
    private void Start()
    {
        t = GetComponent<Transform>();
        if (rotationalVel == 0)
            rotationalVel = 540f;
    }
    public void Update()
    {
        t.eulerAngles = new Vector3(0, 0, t.eulerAngles.z + (rotationalVel * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject g = collision.gameObject;
        if (g.tag == "Player")
        {
            g.GetComponent<PlayerController>().Slice();
        }
    }
}

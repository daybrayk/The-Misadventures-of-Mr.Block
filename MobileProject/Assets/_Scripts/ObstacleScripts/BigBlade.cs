using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlade : MonoBehaviour {
    public float rotationalVel;
    private void Start()
    {
        if (rotationalVel == 0)
            rotationalVel = 540f;
    }
    public void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + (rotationalVel * Time.fixedDeltaTime));
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

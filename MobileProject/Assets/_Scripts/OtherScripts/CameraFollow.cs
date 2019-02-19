using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player;
    public float playerOffset;
    // Use this for initialization
    void Start () {
        if (playerOffset == 0)
            playerOffset = -2.5f;
    }


    private void FixedUpdate()
    {
        if((player.position.x - transform.position.x) >= playerOffset)
            transform.position = new Vector3(Mathf.Clamp(player.position.x - playerOffset, transform.position.x, float.MaxValue), 0, -10);
    }
}

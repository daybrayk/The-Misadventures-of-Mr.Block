using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player;
    float playerOffset;
    // Use this for initialization
    void Start () {
        playerOffset = transform.position.x - player.position.x;
    }


    private void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(player.position.x + playerOffset, transform.position.x, float.MaxValue), 0, -10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player;
    float playerOffset;
    private float _lastPosition;
    public RoofSpawn rs;
    // Use this for initialization
    void Start () {
        playerOffset = transform.position.x - player.position.x;
        _lastPosition = transform.position.x;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(player.position.x + playerOffset, transform.position.x, float.MaxValue), 0, -10);
        if(transform.position.x - _lastPosition >= 1.275f)
        {
            rs.SpawnRoof();
            _lastPosition = transform.position.x;
        }
    }
}

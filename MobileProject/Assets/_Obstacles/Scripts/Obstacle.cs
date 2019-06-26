using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public Transform objectPool;

    private void ReturnToPool()
    {
        transform.position = objectPool.position;
        gameObject.SetActive(false);
    }
}

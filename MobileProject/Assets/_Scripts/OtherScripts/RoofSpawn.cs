using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofSpawn : MonoBehaviour {
    public GameObject roofPrefab;
	public void SpawnRoof()
    {
        Instantiate(roofPrefab, transform.position, transform.rotation);
    }
}

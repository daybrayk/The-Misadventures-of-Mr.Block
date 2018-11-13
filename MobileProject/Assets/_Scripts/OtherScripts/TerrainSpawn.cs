using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawn : MonoBehaviour {
    public Transform[] terrainSpawners;
    public GameObject terrain;
    public GameObject ropeCutter;
    private float spawnTime = 2.0f;
    private float spawnTimer;
    private float cutterTime = 10.0f;
    private float cutterTimer;
	// Use this for initialization
	void Start () {
        spawnTimer = 2.1f;
        cutterTimer = 10.1f;
	}
	
	// Update is called once per frame
	void Update () {
        spawnTimer += Time.deltaTime;
        cutterTimer += Time.deltaTime;
        if (spawnTimer >= spawnTime)
        {
            foreach(Transform i in terrainSpawners)
            {
                if (Random.Range(0, 4) <= 1)
                {
                    if(cutterTimer >= cutterTime)
                    {
                        Instantiate(ropeCutter, i.position, i.rotation);
                        cutterTimer = 0;
                    }
                    else
                        Instantiate(terrain, i.position, i.rotation);
                }
                    
            }
            spawnTimer = 0f;
        }

	}
}

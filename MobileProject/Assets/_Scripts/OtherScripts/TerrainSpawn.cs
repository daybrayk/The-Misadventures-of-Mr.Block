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
		
	}
	
	// Update is called once per frame
	void Update () {
        spawnTimer += Time.deltaTime;
        cutterTimer += Time.deltaTime;
        if (spawnTimer > spawnTime)
        {
            foreach(Transform i in terrainSpawners)
            {
                if (Random.Range(1, 4) == 1)
                    Instantiate(terrain, i.position, i.rotation);
            }
            spawnTimer = 0f;
        }

        if(cutterTimer > cutterTime)
        {
            int random = Random.Range(0, 4);
            Instantiate(ropeCutter, terrainSpawners[random].transform.position, ropeCutter.transform.rotation);
        }

	}
}

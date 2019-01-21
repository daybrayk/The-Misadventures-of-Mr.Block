using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour {
    /*************** Reference Variables ***************/
    public Transform spawnerLocation;
    public Transform player;
   
    /*************** Obstacle Spawn Variables ***************/
    private float _spawnDistance;
    private Vector2 lastPosition;
    private Vector2 playerPosition;
    private float pistonCD;
    private float pistonTimer;
    private float diagonalCD;
    private float diagonalTimer;

    /*************** Obstacles ***************/
    public List<GameObject> obstacles;
    private GameObject currentlyActive;

    // Use this for initialization
    void Start () {
        _spawnDistance = 10.0f;
        lastPosition = playerPosition;

        int num = Random.Range(0, 5);
        //currentlyActive = PlaceAndActivate(obstacles[Random.Range(0, obstacles.Count)], spawnerLocation);
    }
	
	// Update is called once per frame
	void Update () {
        playerPosition = new Vector2(player.position.x, player.position.y);
        if((playerPosition.x - lastPosition.x) >= 25.0f)
        {
            int num = Random.Range(0, 5);
            if(currentlyActive)
                currentlyActive.SetActive(false);
            currentlyActive = PlaceAndActivate(obstacles[Random.Range(0, obstacles.Count)], spawnerLocation);
            lastPosition = playerPosition;
        }
    }

    private GameObject PlaceAndActivate(GameObject obstacle, Transform position)
    {
        obstacle.SetActive(true);
        obstacle.transform.position = position.position;
        return obstacle;
    }
}

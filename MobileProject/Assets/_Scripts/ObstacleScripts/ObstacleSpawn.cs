using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour {
    /*************** Reference Variables ***************/
    public List<Transform> obstacleSpawners;
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
    public List<GameObject> singleGOTopTop;
    public List<GameObject> singleGOTop;
    public List<GameObject> compoundGO;
    public List<GameObject> singleGOBot;
    public List<GameObject> singleGOBotBot;
    private GameObject currentlyActive;

    /*************** Old Variables ***************/
    private float spawnTime = 2.0f;
    private float spawnTimer;

    // Use this for initialization
    void Start () {
        _spawnDistance = 10.0f;
        lastPosition = playerPosition;

        int num = Random.Range(0, 5);
        switch (num)
        {
            case 0:
                currentlyActive = PlaceAndActivate(singleGOTopTop[Random.Range(0, singleGOTopTop.Count)], obstacleSpawners[0]);
                break;
            case 1:
                currentlyActive = PlaceAndActivate(singleGOTop[Random.Range(0, singleGOTop.Count)], obstacleSpawners[1]);
                break;
            case 2:
                currentlyActive = PlaceAndActivate(compoundGO[Random.Range(0, compoundGO.Count)], obstacleSpawners[2]);
                break;
            case 3:
                currentlyActive = PlaceAndActivate(singleGOBot[Random.Range(0, singleGOBot.Count)], obstacleSpawners[3]);
                break;
            case 4:
                currentlyActive = PlaceAndActivate(singleGOBotBot[Random.Range(0, singleGOBotBot.Count)], obstacleSpawners[4]);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        playerPosition = new Vector2(player.position.x, player.position.y);
        if((playerPosition.x - lastPosition.x) >= 25.0f)
        {
            int num = Random.Range(0, 5);
            currentlyActive.SetActive(false);
            //Debug.Log(singleGOBot.Count + " " + singleGOTop.Count + " " + singleGOBotBot.Count + " " + singleGOTopTop.Count);
            switch(num)
            {
                case 0:
                    currentlyActive = PlaceAndActivate(singleGOTopTop[Random.Range(0, singleGOTopTop.Count )], obstacleSpawners[0]);
                    break;
                case 1:
                    currentlyActive =  PlaceAndActivate(singleGOTop[Random.Range(0, singleGOTop.Count)], obstacleSpawners[1]);
                    break;
                case 2:
                    currentlyActive = PlaceAndActivate(compoundGO[Random.Range(0, compoundGO.Count)], obstacleSpawners[2]);
                    break;
                case 3:
                    currentlyActive = PlaceAndActivate(singleGOBot[Random.Range(0, singleGOBot.Count)], obstacleSpawners[3]);
                    break;
                case 4:
                    currentlyActive = PlaceAndActivate(singleGOBotBot[Random.Range(0, singleGOBotBot.Count)], obstacleSpawners[4]);
                    break;
            }
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

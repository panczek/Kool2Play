using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Script for spawning enemies and boosts for the player

public class Spawner : MonoBehaviour
{
    public float delayBetweenSpawn = 1f;
    public float DistanceToThePlayer = 20f; //if player is in the distance, the spawner wont spawn
    private float delayCounter = 0;
    public GameObject enemy;
    public GameObject enemyParent;
    private GameObject player;
    public Transform cornerLT;
    public Transform cornerRT;
    public Transform cornerLB;
    public Transform cornerRB;

    public bool dropBoosts = true;
    public GameObject[] boosts;
    public GameObject boostsParent;
    public float boostCounter = 10f;
    private float boostActualCounter;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boostActualCounter = boostCounter;
    }

    // Update is called once per frame
    void Update()
    {
        delayCounter += Time.deltaTime;
        
        if (delayCounter > delayBetweenSpawn)
        {
            //picking random wall for the spawn point
            int RandomWall = Random.Range(0, 4);
            Vector3 spawnPos = cornerLT.position;
            if (RandomWall == 0)
            {
                spawnPos = PickWall(cornerLT, cornerRT, 'z');
            }
            else if (RandomWall == 1)
            {
                spawnPos = PickWall(cornerRT, cornerRB, 'x');
            }
            else if (RandomWall == 2)
            {
                spawnPos = PickWall(cornerRB, cornerLB, 'z');
            }
            else if (RandomWall == 3)
            {
                spawnPos = PickWall(cornerLB, cornerLT, 'x');
            }
            if(player != null)
            {
                if (Vector3.Distance(spawnPos, player.transform.position) > DistanceToThePlayer)
                {
                    GameObject tempEnemy =  Instantiate(enemy, spawnPos, Quaternion.identity);
                    tempEnemy.transform.parent = enemyParent.transform;
                    delayCounter = 0f;
                }
            }  
        }
        if (dropBoosts)//if boost dropping is enabled
        {
            boostActualCounter -= Time.deltaTime;
            if(boostActualCounter <= 0f)
            {
                SpawnBoost();
            }
        }
            
        

    }
    Vector3 PickWall(Transform point1, Transform point2, char axis)
    {
        Vector3 returnValue = point1.position;
        float value1;
        float value2;
        if(axis == 'x')
        {
            value1 = point1.transform.position.z;
            value2 = point2.transform.position.z;

            float randomZ = Random.Range(value1, value2);
            returnValue.z = randomZ;
        }
        else if (axis == 'z')
        {
            value1 = point1.transform.position.x;
            value2 = point2.transform.position.x;

            float randomX = Random.Range(value1, value2);
            returnValue.x = randomX;
        }
        return returnValue;
    }
    void SpawnBoost()
    {
        boostActualCounter = boostCounter;
        int random = Random.Range(0, boosts.Count());
        float randomX = Random.Range(cornerLT.position.x, cornerRT.position.x);
        float randomZ = Random.Range(cornerLB.position.z, cornerLT.position.z);
        Vector3 spawnPos = new Vector3(randomX, 25f, randomZ);
        GameObject tempBoost =  Instantiate(boosts[random], spawnPos, Quaternion.identity);
        tempBoost.transform.parent = boostsParent.transform;
    }
}

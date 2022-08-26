using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static int nbEnemies = 0;

    private float lastSpawn;
    public GameObject[] enemiesClones = new GameObject[nbEnemies];
    public GameObject player;
    private int distanceToPlayer;

    void Start()
    {
        lastSpawn = Time.time;
        distanceToPlayer = 25;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSpawn + nbEnemies * 2 + 2 < Time.time && nbEnemies < 10)
        {
            // spawn enemy
            int sideSpawn = Random.value < 0.5 ? distanceToPlayer : -distanceToPlayer;
            Instantiate(enemiesClones[Random.Range(0, enemiesClones.Length)], new Vector2(player.gameObject.transform.position.x + sideSpawn, 4f), Quaternion.identity);
            nbEnemies++;
            lastSpawn = Time.time;
        }
    }

    public static void EnemyKilled()
    {
        nbEnemies--;
    }
}

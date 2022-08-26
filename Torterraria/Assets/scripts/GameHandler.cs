using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static int nbEnemies = 2;

    private float lastSpawn;
    public GameObject[] enemiesClones = new GameObject[nbEnemies];
    public GameObject player;

    void Start()
    {
        lastSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSpawn + nbEnemies * 2 + 2 < Time.time)
        {
            // spawn enemy
            int sideSpawn = Random.value < 0.5 ? 10 : -10;
            Instantiate(enemiesClones[Random.Range(0, enemiesClones.Length)], new Vector2(player.gameObject.transform.position.x + sideSpawn, player.gameObject.transform.position.y + 4f), Quaternion.identity);
            nbEnemies++;
            lastSpawn = Time.time;
        }
    }

    public static void EnemyKilled()
    {
        nbEnemies--;
    }
}
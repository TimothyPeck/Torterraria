using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int WIDTH = 150;
    public const int HEIGHT = 200;
    public static int cpt = 0;

    public GameObject[] enemiesClones = new GameObject[nbEnemies];
    public GameObject player;

    private static int nbEnemies = 0;
    private float lastSpawn;
    private int distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Spawn").transform.position = new Vector3(-WIDTH + 5, 5, 0);
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("Spawn").transform.position;
        //CreateGround();

        lastSpawn = Time.time;
        distanceToPlayer = 35;
    }

    void Update()
    {
        if (lastSpawn + nbEnemies * 2 + 2 < Time.time && nbEnemies < 5)
        {
            // spawn enemy
            int spawnDistance = Random.Range(distanceToPlayer, distanceToPlayer + 10);
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

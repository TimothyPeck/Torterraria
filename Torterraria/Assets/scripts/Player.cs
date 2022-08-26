using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int baseHealth;
    public int health;
    public HealthBar healthBar;
    public Vector2 spawnPosition;

    // eject
    private float lastCollision;

    // Start is called before the first frame update
    void Start()
    {
        health = baseHealth;
        gameObject.transform.position = spawnPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //Health regen
        if (Time.time - lastCollision > 5 && health < baseHealth)
        {
            health++;
            healthBar.UpdateHealthBar();
            lastCollision += 1;
        }

        if (Input.GetKeyDown("q"))
        {
            PlayerHit(baseHealth / 2);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if(PlayerHit(collision.gameObject.GetComponent<Enemy>().damage) == false)
            {
                gameObject.GetComponent<movePlayer>().DamageForce(transform.position - collision.gameObject.transform.position);
            }
        }
        else if (collision.gameObject.tag == "Item")
        {
            GameObject.Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Enemy1Loot")
        {
            GameObject.Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Enemy2Loot")
        {
            GameObject.Destroy(collision.gameObject);
        }
    }


    bool PlayerHit(int damage)
    {
        bool isDead = false;
        // Immunity after hit (in seconds)
        if (Time.time - lastCollision > 1.5)
        {
            health -= damage;
            lastCollision = Time.time;
        }
        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            gameObject.transform.position = GameObject.Find("Spawn").transform.position;
            health = baseHealth;
            isDead = true;
        }
        healthBar.UpdateHealthBar();
        return isDead;
    }
}

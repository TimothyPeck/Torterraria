using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int baseHealth;
    public int health;
    public HealthBar healthBar;

    // eject
    private float lastCollision;

    // Start is called before the first frame update
    void Start()
    {
        health = baseHealth;
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
            // simulate an attack temporary
            if (Input.GetKey("left shift"))
            {
                if (collision.gameObject.GetComponent<Enemy>().GettingAttacked(1) == false)
                {
                    this.gameObject.GetComponent<movePlayer>().DamageForce(this.transform.position - collision.gameObject.transform.position);
                }
            }
            else
            {
                PlayerHit(collision.gameObject.GetComponent<Enemy>().damage);
                if (health <= 0)
                {
                    
                }
                else
                {
                    this.gameObject.GetComponent<movePlayer>().DamageForce(this.transform.position - collision.gameObject.transform.position);
                }
            }
        }
    }


    void PlayerHit(int damage)
    {
        // Immunity after hit (in seconds)
        if (Time.time - lastCollision > 1.5)
        {
            health -= damage;
            lastCollision = Time.time;
        }
        if (health <= 0)
        {
            print("Dead");
            gameObject.transform.position = new Vector2(8, -0.5f);
            health = baseHealth;
        }
        Debug.Log(health);
        healthBar.UpdateHealthBar();
    }
}

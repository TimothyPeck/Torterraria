using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int baseHealth;
    public int health;
    public HealthBar healthBar;
    private int timeSinceLastHit;
    // Start is called before the first frame update
    void Start()
    {
        health = baseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Increments time since last hit
        if (timeSinceLastHit < 2000)
            timeSinceLastHit++;

        //Health regen
        if (timeSinceLastHit > 1000 && health < baseHealth)
        {
            health++;
            healthBar.UpdateHealthBar();
        }

        if (Input.GetKeyDown("q"))
        {
            PlayerHit(baseHealth / 2);
        }
    }

    void PlayerHit(int damage)
    {
        // Immunity after hit
        if (timeSinceLastHit > 1000)
        {
            health -= damage;
            timeSinceLastHit = 0;
        }
        if (health < 0)
        {
            print("Dead");
        }
        healthBar.UpdateHealthBar();
    }
}

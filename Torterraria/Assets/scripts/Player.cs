using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int baseHealth;
    public int health;
    public HealthBar healthBar;
    public Vector2 spawnPosition;
    public GameObject canvas;

    public bool canJump;
    public float lastGroundContact;

    // eject
    private float lastCollision;

    // Start is called before the first frame update
    void Start()
    {
        health = baseHealth;
        gameObject.transform.position = spawnPosition;
        canJump = true;
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
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if(PlayerHit(collision.gameObject.GetComponent<Enemy>().damage) == false)
            {
                gameObject.GetComponent<movePlayer>().DamageForce(transform.position - collision.gameObject.transform.position);
            }
        }
        else if (collision.gameObject.tag == "Item")
        {
            // Add the item picked up to the inventory
            int cpt = 0;
            foreach (string ressource in Inventory.RessourcesName)
            {
                string[] collisionName = collision.gameObject.name.Split("_");
                if (ressource == collisionName[0])
                {
                    Inventory.indexRessource = cpt;
                    canvas.GetComponent<Inventory>().CollectResource();
                }
                cpt++;
            }

            GameObject.Destroy(collision.gameObject);
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
            lastGroundContact = Time.time;
        }
        //print("collision: " + collision.gameObject.name);
        if (collision.gameObject.name.Contains("top") && Input.GetAxisRaw("Vertical") < 0)
        {
            StartCoroutine(MakePlatformTraversable(.42f, collision));
        }
    }

    IEnumerator MakePlatformTraversable(float seconds, Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        collidedObject.GetComponent<MeshCollider>().enabled = false;
        yield return new WaitForSecondsRealtime(seconds);
        collidedObject.GetComponent<MeshCollider>().enabled = true;
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

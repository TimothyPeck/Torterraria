using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage;
    public int healthMax;
    public int speed;
    public float dropRate;
    public GameObject lootToDrop;
    public float secondsInDirection;
    public GameObject player;

    private float speedX;
    private float timeSpent;
    private Rigidbody rb;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        speedX = speed;
        timeSpent = 0f;
        rb = GetComponent<Rigidbody>();
        health = healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        timeSpent += Time.deltaTime;
        if (timeSpent >= secondsInDirection)
        {
            speedX *= -1;
            timeSpent -= secondsInDirection;
        }
        float distance = gameObject.transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x;
        if (Mathf.Abs(distance) > 5f)
        {
            rb.velocity = new Vector2(speedX, rb.velocity.y);
        }
        else if (Mathf.Abs(distance) > 1f)
        {
            float i = -distance / Mathf.Abs(distance);
            rb.velocity = new Vector2(i * speed, rb.velocity.y);
        }
        //transform.position += moveVector * Time.deltaTime;
    }
    void OnCollisionEnter(Collision collision)
    {

    }
    public bool GettingAttacked(int damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
        {
            GameHandler.EnemyKilled();
            if (Random.value < dropRate)
            {
                Debug.Log("Loot dropped");
            }
            GameObject.Destroy(gameObject);
            return true;
        }
        return false;
    }
}

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

    private float speedX;
    private float timeSpent;
    private Rigidbody rb;
    private int health;

    // eject
    private Vector2 collisionDirection;
    private float lastCollision;
    private const float ejectDuration = 0.5f;

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

        Vector2 collisionForce = new(0, 0);
        if (Time.time - lastCollision < ejectDuration)
        {
            collisionForce = collisionDirection * (ejectDuration - Time.time + lastCollision) / ejectDuration;
        }
        collisionForce.y = Mathf.Max(collisionForce.y, 0);

        float distance = Vector2.Distance(gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        float forceY = collisionForce.y <= 0 ? rb.velocity.y : collisionForce.y;
        if(Mathf.Abs(distance) > 30f)
        {
            GameObject.Destroy(gameObject);
        }
        else if (Mathf.Abs(distance) > 8f)
        {
            rb.velocity = new Vector2(speedX + collisionForce.x, forceY);
        }
        else if (Mathf.Abs(distance) > 1f)
        {
            float i = gameObject.transform.position.x > GameObject.FindGameObjectWithTag("Player").transform.position.x ? -1 : 1;
            rb.velocity = new Vector2(i * speed + collisionForce.x, forceY);
        }
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
        collisionDirection = (this.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position);
        collisionDirection = collisionDirection / collisionDirection.magnitude * 20;
        lastCollision = Time.time;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage;
    public int speed;
    public float dropRate;
    public GameObject lootToDrop;
    public HealthBar healthBar;
    public float secondsInDirection;

    public GameObject objectHealthBar;

    private float speedX;
    private float timeSpent;
    private Rigidbody rb;
    private Health h;

    private float lastJump;
    private float spawnTime;

    private bool isFacingRight = true;

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
        h = GetComponent<Health>();

        h.health = h.baseHealth;

        spawnTime = Time.time;
        lastJump = Time.time;

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

        float forceX = 0;
        float forceY = collisionForce.y <= 0 ? rb.velocity.y : collisionForce.y;

        // distance between the player and the enemy
        float distance = Vector2.Distance(gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        float distanceX = Mathf.Abs(gameObject.transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x);
        float distanceY = GameObject.FindGameObjectWithTag("Player").transform.position.y - gameObject.transform.position.y;

        if (Mathf.Abs(distance) > 55f) // if too far away from the player, disappear forever
        {
            GameManager.EnemyKilled();
            GameObject.Destroy(gameObject);
        }
        else if (distance > 8f)
        {
            forceX = speedX + collisionForce.x;

        }
            
        else
        {
            string[] nameEnemy = gameObject.name.Split("(");
            Debug.Log(nameEnemy[0]);
            switch (nameEnemy[0])
            {
                case "Enemy1":
                    SfxManager.instance.audio.PlayOneShot(SfxManager.instance.enemy1);
                    break;
                case "Enemy2":
                    SfxManager.instance.audio.PlayOneShot(SfxManager.instance.enemy2);
                    break;
                case "Enemy3":
                    SfxManager.instance.audio.PlayOneShot(SfxManager.instance.enemy3);
                    break;
                case "Enemy4":
                    SfxManager.instance.audio.PlayOneShot(SfxManager.instance.enemy4);
                    break;
                case "Boss":
                    SfxManager.instance.audio.PlayOneShot(SfxManager.instance.boss);
                    break;
                default:
                    break;
            }
            if (distanceY > 1f && Time.time - lastJump > 2f)
            {
                lastJump = Time.time;
                forceY += 5;
            }
            if (distanceX > 0.9f) // if close enough, run to the player
            {
                float i = gameObject.transform.position.x > GameObject.FindGameObjectWithTag("Player").transform.position.x ? -1 : 1;
                forceX = i * speed + collisionForce.x;
            }
        }

        rb.velocity = new Vector2(forceX, forceY);


        if (rb.velocity.x < -1 && isFacingRight == true)
        {
            isFacingRight = false;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            objectHealthBar.transform.localScale = objectHealthBar.transform.localScale * -1;
        }
        else if (rb.velocity.x > -1 && isFacingRight == false)
            {
            isFacingRight = true;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            objectHealthBar.transform.localScale = objectHealthBar.transform.localScale * -1;
        }
    }

    public bool GettingAttacked(int damageTaken)
    {
        h.health -= damageTaken;
        healthBar.UpdateHealthBar();
        SfxManager.instance.audio.PlayOneShot(SfxManager.instance.enemyHit);
        if (h.health <= 0)
        {
            GameManager.EnemyKilled();
            if (Random.value < dropRate)
            {
                GameObject.Instantiate(lootToDrop, gameObject.transform.position, Quaternion.identity);
            }

            SfxManager.instance.audio.PlayOneShot(SfxManager.instance.enemyDeath);
            GameObject.Destroy(gameObject); 

            return true;
            
        }

        collisionDirection = (this.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position);
        collisionDirection = collisionDirection / collisionDirection.magnitude * 8;
        lastCollision = Time.time;
        return false;
    }
}

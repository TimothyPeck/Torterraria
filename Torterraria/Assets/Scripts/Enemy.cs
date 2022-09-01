using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage;
    public int speed;

    public float dropRate;
    public float secondsInDirection;

    public GameObject lootToDrop;
    public GameObject objectHealthBar;

    public HealthBarScript healthBar;

    private float speedX;
    private float timeSpent;
    private float lastJump;
    private float lastCollision;

    private const float ejectDuration = 0.5f;

    private Rigidbody rb;

    private Health h;

    private bool isFacingRight = true;

    private Vector2 collisionDirection;

    private Vector3 theScale;

    private bool bossAlreadySeen;

    // Start is called before the first frame update
    void Start()
    {
        speedX = speed;

        timeSpent = 0f;

        rb = GetComponent<Rigidbody>();
        h = GetComponent<Health>();

        h.health = h.baseHealth;

        lastJump = Time.time;

        bossAlreadySeen = false;

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
        if (distance < 15f && name == "Boss" && bossAlreadySeen == false)
        {
            Dialogue dialogue = new Dialogue();
            dialogue.AddSentence(GameManager.PLAYER_NAME, "OH MY GOD!! It looks monstrous, it must be the root of all evil!", 6);
            dialogue.AddSentence(GameManager.PLAYER_NAME, "A simple sword or my bare hands won’t do anything against this beast!", 7);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            bossAlreadySeen = true;
        }
        if (distance > 55f && name != "Boss") // if too far away from the player, disappear forever
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

            switch (nameEnemy[0])
            {
                case "Enemy1":
                    SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.enemy1);
                    break;
                case "Enemy2":
                    SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.enemy2);
                    break;
                case "Enemy3":
                    SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.enemy3);
                    break;
                case "Enemy4":
                    SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.enemy4);
                    break;
                case "Boss":
                    SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.boss);
                    break;
                default:
                    break;
            }

            if ((distanceY > 0.7f || name == "Boss") && Time.time - lastJump > 2f)
            {
                lastJump = Time.time;

                forceY += 5;
            }

            if (distanceX > 0.5f) // if close enough, run to the player
            {
                float i = gameObject.transform.position.x > GameObject.FindGameObjectWithTag("Player").transform.position.x ? -1 : 1;
                
                forceX = i * speed + collisionForce.x;
            }
        }

        rb.velocity = new Vector2(forceX, forceY);

        // Puts the player in the correct direction according to their movement

        if (rb.velocity.x < -1 && isFacingRight == true)
        {
            isFacingRight = false;

            theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;

            objectHealthBar.transform.localScale = objectHealthBar.transform.localScale * -1;
        }
        else if (rb.velocity.x > -1 && isFacingRight == false)
        {
            isFacingRight = true;

            theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;

            objectHealthBar.transform.localScale = objectHealthBar.transform.localScale * -1;
        }
    }

    /// <summary>
    /// The enemy takes damage
    /// </summary>
    /// <param name="damageTaken"></param>
    /// <returns></returns>
    public bool GettingAttacked(int damageTaken)
    {
        h.health -= damageTaken;

        healthBar.UpdateHealthBar();

        SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.enemyHit);

        if (h.health <= 0)
        {
            GameManager.EnemyKilled();

            if (Random.value < dropRate)
            {
                GameObject.Instantiate(lootToDrop, gameObject.transform.position + new Vector3(0, 0, -0.4f), Quaternion.identity);
            }

            SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.enemyDeath);

            GameObject.Destroy(gameObject); 

            return true;
            
        }

        collisionDirection = (this.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position);
        collisionDirection = collisionDirection / collisionDirection.magnitude * 8;

        lastCollision = Time.time;

        return false;
    }
}

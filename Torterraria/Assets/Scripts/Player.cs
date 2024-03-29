using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBarScript healthBar;

    public Vector2 spawnPosition;

    public GameObject canvas;

    public bool canJump;

    public float lastGroundContact;

    private Dialogue dialogue = new Dialogue();

    private bool firstTime = true;

    private float lastCollision;

    private Health h;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        h = GetComponent<Health>();

        gameObject.transform.position = GameObject.Find("Spawn").transform.position;

        canJump = true;

        spriteRenderer = GetComponent<SpriteRenderer>();

        lastCollision = Time.time - 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstTime)
        {
            firstTime = !firstTime;
            dialogue.AddSentence(GameManager.PLAYER_NAME, "Something feels off, this Torterra used to be so calm and now evil Pok�mon are everywhere�", 8);
            dialogue.AddSentence(GameManager.PLAYER_NAME, "I have to investigate.", 3);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
        //Health regen
        if (Time.time - lastCollision > 5 && h.health < h.baseHealth)
        {
            h.health++;

            healthBar.UpdateHealthBar();

            lastCollision += 1;
        }

        if(Time.time - lastCollision < 1.5) // damage animation, shows invincibility of the player
        {
            if (Mathf.RoundToInt((Time.time - lastCollision) * 10) % 2 == 1)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }

    /// <summary>
    /// Manages the enter collisions
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // Add the item picked up to the inventory
            int cpt = 0;

            string[] collisionName = collision.gameObject.name.Split("_");

            foreach (string ressource in Inventory.ressourcesName)
            {
                if (ressource == collisionName[0])
                {
                    Inventory.indexRessource = cpt;

                    canvas.GetComponent<Inventory>().CollectResource();
                }
                cpt++;
            }

            if (collisionName[0] == "crown")
            {
                Dialogue dialogue = new Dialogue();
                dialogue.AddSentence(GameManager.PLAYER_NAME, "Wow, looks like I�m the new king now.", 4);
                dialogue.AddSentence(GameManager.PLAYER_NAME, "Everything should come back to normal, I�ll protect this world from this day �till my last, I swear!", 7);
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            }

            canvas.GetComponent<Inventory>().ClickedSlot(-1);

            GameObject.Destroy(collision.gameObject);
        }
        
    }

    /// <summary>
    /// Manages the stay collisions
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag.StartsWith("Super"))
        {
            Vector2 vector = transform.position - collision.transform.position;

            if(vector.y > 0 && 0.5 * vector.y > Mathf.Abs(vector.x))
            {
                canJump = true;

                lastGroundContact = Time.time;
            }
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (PlayerHit(collision.gameObject.GetComponent<Enemy>().damage) == false)
            {
                gameObject.GetComponent<MovePlayer>().DamageForce(transform.position - collision.gameObject.transform.position);
            }
        }

        if ((collision.gameObject.name.Contains("platform") || collision.gameObject.name.Contains("top")) && Input.GetAxisRaw("Vertical") < 0)
        {
            StartCoroutine(MakePlatformTraversable(.7f, collision));
        }
    }

    /// <summary>
    /// Manages the platform logic
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="collision"></param>
    /// <returns></returns>
    IEnumerator MakePlatformTraversable(float seconds, Collision collision)
    {
        // Saves the collided object to not cause problems if the player touches another object in seconds.
        GameObject collidedObject = collision.gameObject;

        // Let's player pass through
        collidedObject.GetComponent<MeshCollider>().enabled = false;

        // Wait for seconds
        yield return new WaitForSecondsRealtime(seconds);

        // Stop player again.
        collidedObject.GetComponent<MeshCollider>().enabled = true;
    }

        
    /// <summary>
    /// The player takes damage
    /// </summary>
    /// <param name="damage">the ammount of damage</param>
    /// <returns>if the player is dead or not</returns>
    bool PlayerHit(int damage)
    {
        SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.playerHit);

        bool isDead = false;

        // Immunity after hit (in seconds)

        if (Time.time - lastCollision > 1.5)
        {
            h.health -= damage;
            lastCollision = Time.time;
        }

        if (h.health <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            gameObject.transform.position = GameObject.Find("Spawn").transform.position;

            h.health = h.baseHealth;

            SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.playerDeath);

            isDead = true;
        }
        healthBar.UpdateHealthBar();

        return isDead;
    }
}

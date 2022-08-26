using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class movePlayer : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float moveSpeed;
    private Vector3 moveVector;
    private Rigidbody rb;
    private float jumpForce = 5;
    private Player player;

    // eject
    private Vector2 collisionDirection;
    private float lastCollision;
    private const float ejectDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the current move speed as being the base speed of the player. The move speed is modified when sprinting so need to keep base speed.
        moveSpeed = baseSpeed;
        //Gets the Rigidbody of the current object (the player)
        rb = GetComponent<Rigidbody>();
        // The vector used to move the player, assigned to none so the player is not moving at the start.
        moveVector = new Vector3(0, 0, 0);
        player = gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //Teleports the player back to spawn if they fall out of the world somehow
        if (rb.transform.position.y < -GameManager.HEIGHT - 10)
        {
            rb.transform.position = new Vector3(0, 10, 0);
        }

        // calculate the force of the collision with an enemy
        Vector2 collisionForce = new(0, 0);
        if (Time.time - lastCollision < ejectDuration)
        {
            collisionForce = collisionDirection * (ejectDuration - Time.time + lastCollision) / ejectDuration;
        }

        //Jumps if the space bar is pressed and the player is not jumping or falling.
        if (Input.GetKeyDown(KeyCode.Space) && player.canJump == true)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            player.canJump = false;
        }

        //doubles the speed of the player if shift is held down (sprint)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = baseSpeed * 2;
        }
        else
        {
            moveSpeed = baseSpeed;
        }

        //Sets the x factor of the movement to the move speed times the direction. Left = -1,  Right = 1
        moveVector.x = moveSpeed * Input.GetAxisRaw("Horizontal") + collisionForce.x;
        if (collisionForce.y > 0)
        {
            moveVector.y = collisionForce.y;
        }
        else
        {
            moveVector.y = rb.velocity.y;
        }

        //Sets the velocity of the rigidbody to a new vector with the current move vector
        rb.velocity = new Vector3(moveVector.x, moveVector.y);
    }

    public void DamageForce(Vector2 direction)
    {
        direction = direction / direction.magnitude * 20;
        if (direction.y < 0)
        {
            direction.y = 0;
        }
        collisionDirection = direction;
        lastCollision = Time.time;
    }

    
}

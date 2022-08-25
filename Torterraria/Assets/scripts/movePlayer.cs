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
    public float buttonTime = 0.3f;
    float jumpTime;
    bool jumping;

    // eject
    private Vector2 collisionDirection;
    private float lastCollision;
    private const float ejectDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = baseSpeed;
        rb = GetComponent<Rigidbody>();
        moveVector = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.transform.position.y < -GameManager.HEIGHT - 10)
        {
            rb.transform.position = new Vector3(0, 10, 0);
        }
        float jumpForce = 5;

        // calculate the forcer of the collision with an enemy
        Vector2 collisionForce = new(0, 0);
        if (Time.time - lastCollision < ejectDuration)
        {
            collisionForce = collisionDirection * (ejectDuration - Time.time + lastCollision) / ejectDuration;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) <= 0.5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        }

        moveVector.x = moveSpeed * Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = baseSpeed * 2;
        }
        else
        {
            moveSpeed = baseSpeed;
        }
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y);
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

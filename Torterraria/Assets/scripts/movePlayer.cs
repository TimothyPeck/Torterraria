using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
    private float moveSpeed = 5f;
    private Vector3 moveVector;
    private Rigidbody rb;
    public float buttonTime = 0.3f;
    float jumpTime;
    bool jumping;

    // Start is called before the first frame update
    void Start()
    {
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


        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        }
        moveVector.x = moveSpeed * Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y);

    }

    
}

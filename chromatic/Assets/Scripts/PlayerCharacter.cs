using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public float movementSpeed = 5.0f;          // The speed at which the plyer moves.
    //public float maxFallSpeed = 10.0f;          // The player's terminal velocity.
    //public float jumpForce = 100.0f;            // The amount of force exterted upwards when the player jumps.

    private Rigidbody2D rb;                     // The player's rigidbody component.
    //private bool applyJumpForce = false;        // Informs fixed update if jump force should be added.

    /* Use this for initialization. */
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /* Update is called once per frame. */
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Movement();
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Jump();
        //}
    }

    /* FixedUpdate is called once per physics tick. */
    private void FixedUpdate()
    {
        // Applies jumping force to the player.
        //if (applyJumpForce)
        //{
        //    applyJumpForce = false;

        //    float force;
        //    if (rb.velocity.y < 0)
        //    {
        //        // Scales the force based on falling velocity.
        //        force = Mathf.Abs(rb.velocity.y) + jumpForce;
        //    } 
        //    else
        //    {
        //        force = jumpForce;
        //    }

        //    rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        //}

        // Stops the player from continually gaining speed.
        //rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxFallSpeed);
    }

    /* Player movement. */
    private void Movement()
    {
        float direction = Input.GetAxis("Horizontal");
        if (direction > 0)
            rb.MovePosition(transform.position + transform.right * movementSpeed * Time.deltaTime);
        else if (direction < 0)
            rb.MovePosition(transform.position + -transform.right * movementSpeed * Time.deltaTime);

        //Vector3 movement = Vector3.zero;
        //movement.x = Input.GetAxis("Horizontal");
        //transform.position += movement * movementSpeed * Time.deltaTime;
    }

    /* Indicates that the player should jump to FixedUpdate. */
    //private void Jump()
    //{
    //    applyJumpForce = true;
    //}

    /* Behaviour for when a collision occurs. */
    private void OnCollisionEnter2d(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            // Slow game time and end game.
        }
    }
}
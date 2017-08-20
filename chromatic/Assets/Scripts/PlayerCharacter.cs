using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public float maxFallSpeed = 10.0f;          // The player's terminal velocity.
    public float jumpForce = 100.0f;            // The amount of force exterted upwards when the player jumps.

    private Rigidbody rb;                       // The player's rigidbody component.
    private bool applyJumpForce = false;        // Informs fixed update if jump force should be added.

    /* Use this for initialization. */
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /* Update is called once per frame. */
    private void Update()
    {
        Jump();
    }

    /* FixedUpdate is called once per physics tick. */
    private void FixedUpdate()
    {
        // Applies jumping force to the player.
        if (applyJumpForce)
        {
            applyJumpForce = false;

            float force;
            if (rb.velocity.y < 0)
            {
                // Scales the force based on falling velocity.
                force = Mathf.Abs(rb.velocity.y) + jumpForce;
            } 
            else
            {
                force = jumpForce;
            }

            rb.AddForce(Vector3.up * force);
            Debug.Log(Mathf.Abs(rb.velocity.y));
        }

        // Stops the player from continually gaining speed.
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxFallSpeed);
    }

    /* Causes the player to jump. */
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            applyJumpForce = true;
        }
    }
}
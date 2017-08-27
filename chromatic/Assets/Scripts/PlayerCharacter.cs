using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public float movementSpeed = 5.0f;          // The speed at which the plyer moves.

    private Rigidbody2D rb;                     // The player's rigidbody component.
    private bool movePlayer = false;            // Signals for FixedUpdate to move the player.
    private float moveDirection = 0;            // Positive = right, negative = left.

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
            moveDirection = Input.GetAxis("Horizontal");
            movePlayer = true;
        }
    }

    /* FixedUpdate is called once per physics tick. */
    private void FixedUpdate()
    {
        if (movePlayer)
        {
            movePlayer = false;
            Movement();
        }
    }

    /* Player movement. */
    private void Movement()
    {
        if (moveDirection > 0)
            rb.MovePosition(transform.position + transform.right * movementSpeed * Time.deltaTime);
        else if (moveDirection < 0)
            rb.MovePosition(transform.position + -transform.right * movementSpeed * Time.deltaTime);
    }

    /* Behaviour for when a collision occurs. */
    private void OnCollisionEnter2d(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            // Slow game time and end game.
        }
    }
}
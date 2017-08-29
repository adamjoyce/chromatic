﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject colorManager;                 // The scene's color manager used for changing the background color.
    public GameObject blockManager;                 // The scene's block manager used for adjusting block colors when the background color changes.
    public float movementSpeed = 5.0f;              // The speed at which the plyer moves.

    private Rigidbody2D rb;                         // The player's rigidbody component.
    private bool movePlayer = false;                // Signals for FixedUpdate to move the player.
    private bool updateBlockVisibility = false;     // Signals for FixedUpdate a background color change has occured and the blocks need updating.
    private float moveDirection = 0;                // Positive = right, negative = left.

    /* Use this for initialization. */
    private void Start()
    {
        if (!colorManager) { colorManager = GameObject.Find("ColorManager"); }
        if (!blockManager) { blockManager = GameObject.Find("BlockManager"); }
        rb = GetComponent<Rigidbody2D>();
    }

    /* Update is called once per frame. */
    private void Update()
    {
        // Player movement.
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            moveDirection = Input.GetAxis("Horizontal");
            movePlayer = true;
        }

        // Changing background color.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Color backgroundColor = colorManager.GetComponent<ColorManager>().ChangeBackgroundColor();
            blockManager.GetComponent<BlockManager>().UpdateBlockVisibility(backgroundColor);
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
            rb.MovePosition(transform.position + transform.right * movementSpeed * Time.fixedDeltaTime);
        else if (moveDirection < 0)
            rb.MovePosition(transform.position + -transform.right * movementSpeed * Time.fixedDeltaTime);
    }

    /* Behaviour for when a collision occurs. */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collide");
        if (collision.gameObject.tag == "Block")
        {
            // Slow game time and end game.
            //Time.timeScale = 1f / 10f;
            //Time.fixedDeltaTime = Time.fixedDeltaTime / 10f;
        }
    }
}
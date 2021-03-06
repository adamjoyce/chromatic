﻿using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameManager gameManager;                 // The scene's game manager script used for managing the game's difficulty.
    public ColorManager colorManager;               // The scene's color manager script used for changing the background color.
    public BlockManager blockManager;               // The scene's block manager script used for adjusting block colors when the background color changes.
    public UIManager UIManager;                     // The scene's UI manager script used for deactivating the menu selection elements.
    public float movementSpeed = 5.0f;              // The speed at which the plyer moves.

    private Rigidbody2D rb;                         // The player's rigidbody component.
    private bool movePlayer = false;                // Signals for FixedUpdate to move the player.
    private float moveDirection = 0;                // Positive = right, negative = left.

    private bool blockCollided = false;             // True when the player collides with a block - reset when the next gae begins.

    /* Use this for initialization. */
    private void Start()
    {
        if (!gameManager) { gameManager = FindObjectOfType<GameManager>(); }
        if (!colorManager) { colorManager = FindObjectOfType<ColorManager>(); }
        if (!blockManager) { blockManager = FindObjectOfType<BlockManager>(); }
        if (!UIManager) { UIManager = FindObjectOfType<UIManager>(); }
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
        if (gameManager.IsPlaying)
        {
            if (!colorManager.BackgroundChanged && Input.GetKeyDown(KeyCode.Space))
            {
                if (blockManager.GetSolidLine() && !colorManager.BackgroundChanged)
                {
                    Color backgroundColor = colorManager.ChangeBackgroundColor();
                    FindObjectOfType<AudioManager>().Play("ColorChange");
                    blockManager.UpdateBlockVisibility(backgroundColor);

                    // Slow down time to allow for adjustments.
                    blockManager.ApplyAdjustmentWindow();

                    // Avoid cycling colors more than once per block line.
                    colorManager.BackgroundChanged = true;
                }
                else
                {
                    // Punish background color change spam by increasing the diffiuclty.
                    gameManager.IncreaseDifficulty();
                }
            }
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
        // Block collision.
        if (!blockCollided && collision.gameObject.tag == "Block")
        {
            blockCollided = true;

            // Slow game time and reset game.
            StartCoroutine(gameManager.EndAndResetGame());
        }

        // Menu selection.
        if (!gameManager.IsPlaying)
        {
            blockCollided = false;
            UIManager.ActivateMenuOption(collision.gameObject.transform);
        }
    }
}
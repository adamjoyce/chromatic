using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public GameManager gameManager;                 // The scene's game manager script used for managing the game's difficulty.
    public ColorManager colorManager;               // The scene's color manager script used for changing the background color.
    public BlockManager blockManager;               // The scene's block manager script used for adjusting block colors when the background color changes.
    public float movementSpeed = 5.0f;              // The speed at which the plyer moves.

    private Rigidbody2D rb;                         // The player's rigidbody component.
    private bool movePlayer = false;                // Signals for FixedUpdate to move the player.
    private float moveDirection = 0;                // Positive = right, negative = left.

    /* Use this for initialization. */
    private void Start()
    {
        if (!gameManager) { gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); }
        if (!colorManager) { colorManager = GameObject.Find("ColorManager").GetComponent<ColorManager>(); }
        if (!blockManager) { blockManager = GameObject.Find("BlockManager").GetComponent<BlockManager>(); }
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
        if (!colorManager.GetBackgroundChanged() && Input.GetKeyDown(KeyCode.Space))
        {
            if (blockManager.GetSolidLine() && !colorManager.GetBackgroundChanged())
            {
                Color backgroundColor = colorManager.ChangeBackgroundColor();
                blockManager.UpdateBlockVisibility(backgroundColor);

                // Slow down time to allow for adjustments.
                blockManager.ApplyAdjustmentWindow();

                // Avoid cycling colors more than once per block line.
                colorManager.SetBackgroundChanged(true);
            }
            else
            {
                // Punish background color change spam by increasing the diffiuclty.
                gameManager.IncreaseDifficulty();
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
        if (collision.gameObject.tag == "Block")
        {
            // Slow game time and end game.
            //Time.timeScale = 1f / 10f;
            //Time.fixedDeltaTime = Time.fixedDeltaTime / 10f;
        }
    }
}
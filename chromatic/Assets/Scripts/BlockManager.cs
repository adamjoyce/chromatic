using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject blockPrefab;              // The block that will make up each line.
    public GameManager gameManager;             // The game manager for updating the current score.
    public ColorManager colorManager;           // The color manager in the scene.
    public Renderer backgroundRenderer;         // The background's renderer component - used for block spacing and start position.
    public int numberOfBlocks = 5;              // The number of blocks each line is made up of.

    private GameObject[] blocks;                // The array of blocks.
    private Vector3 spawnPosition;              // The starting spawn location for the line of blocks.
    private float despawnHeight;                // The height at which the block line is off the screen and can be recycled.
    private float startingBlockSpeed;           // The movement speed of the blocks before each slow down from a color change.
    private int enabledBlockIndex = 0;          // The index of a block in the line that is currently enabled, i.e. not the background colour.
    private bool solidLine = false;             // Indicates if there is no block matching the background color thus creating a solid line.

    /* Use this for initialization. */
    private void Start()
    {
        // Grab the manager scripts if it is not assigned in the editor.
        if (!gameManager) { gameManager = FindObjectOfType<GameManager>(); }
        if (!colorManager) { colorManager = FindObjectOfType<ColorManager>(); }

        blocks = new GameObject[numberOfBlocks];

        // X screen position for the bottom left of the background element.
        float backgroundExtentX = Camera.main.WorldToScreenPoint(new Vector3(backgroundRenderer.bounds.min.x, 0, 0)).x;
        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(backgroundExtentX, -1, 10));
        spawnPosition.y -= blockPrefab.GetComponent<Renderer>().bounds.size.y * 0.5f;

        despawnHeight = backgroundRenderer.bounds.size.y * 0.5f;
    }

    /* Update is called once per frame. */
    private void Update()
    {
        if (gameManager.GetIsPlaying())
        {
            if (!blocks[0])
            {
                // The game has just begun.
                SpawnBlocks();
            }
            else
            {
                // The game is in progress.
                if (blocks[enabledBlockIndex].transform.position.y >= (despawnHeight * 0.5f))
                {
                    SpawnBlocks();
                    gameManager.SetLineScore(gameManager.GetLineScore() + 1);
                }
            }
        }
    }

    /* Slows the blocks movement speed - used directly after background color change to allow for player adjustment. */
    public void ApplyAdjustmentWindow()
    {
        StartCoroutine(SlowBlocks());
    }

    /* Updates the current visible blocks dependent on background color. */
    public void UpdateBlockVisibility(Color backgroundColor)
    {
        Color blockColor = new Color();
        for (int i = 0; i < numberOfBlocks; ++i)
        {
            blockColor = blocks[i].GetComponent<SpriteRenderer>().color;
            if (CheckColorAgainstBackground(blockColor))
            {
                DisableBlock(i);
            }
            else if (!blocks[i].activeInHierarchy)
            {
                // Renable the block and set it to be inline with the rest.
                Vector3 newPosition = blocks[enabledBlockIndex].transform.position;
                blocks[i].transform.position = new Vector3(blocks[i].transform.position.x, newPosition.y, 0);
                blocks[i].SetActive(true);
            }
        }
    }

    /* Updates the block lines' movement speed. */
    public void UpdateLinesMovementSpeed(float speedMultiplier)
    {
        for (int i = 0; i < numberOfBlocks; ++i)
        {
            BlockMovement blockMovement = blocks[i].GetComponent<BlockMovement>();
            float newSpeed = blockMovement.GetMovementSpeed() * speedMultiplier;
            blockMovement.SetMovementSpeed(newSpeed);
            startingBlockSpeed = newSpeed;
        }
    }

    /* Returns true if there are no gaps in the block line. */
    public bool GetSolidLine()
    {
        return solidLine;
    }

    /* Spawns a line of blocks. */
    private void SpawnBlocks()
    {
        // Calculate the values needed for the block spacing and positions.
        float blockWidth = blockPrefab.GetComponent<Renderer>().bounds.size.x;
        float gapSize = ((backgroundRenderer.bounds.size.x - (blockWidth * numberOfBlocks)) / (numberOfBlocks + 1));

        // Position the first block away from the border.
        spawnPosition.x += gapSize + (blockWidth * 0.5f);

        // Grab the colors for the blocks.
        List<Color> availableColors = new List<Color>(colorManager.GetColors());

        // Assume a solid line.
        solidLine = true;

        if (!blocks[0])
        {
            // First time spawning the blocks.
            for (int i = 0; i < numberOfBlocks; ++i)
            {
                // Adjust the spawn position based on which block is being spawned in the line.
                Vector3 position = spawnPosition;
                position.x += (i * blockWidth) + (i * gapSize);

                // Get a valid color for the block and remove it from the available list.
                Color blockColor = GetAvailableColor(ref availableColors);

                // Create the block.
                GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity);
                newBlock.GetComponent<SpriteRenderer>().color = blockColor;
                blocks[i] = newBlock;

                // Disable the block if it matches the background color.
                if (CheckColorAgainstBackground(blockColor))
                {
                    DisableBlock(i);
                    solidLine = false;
                }
            }

            // Records the initial block movement speed for the first colour switch.
            startingBlockSpeed = blocks[0].GetComponent<BlockMovement>().GetMovementSpeed();
        }
        else
        {
            // Recycle the existing blocks.
            // Reposition the line.
            for (int i = 0; i < numberOfBlocks; ++i)
            {
                blocks[i].SetActive(false);
                Vector3 position = spawnPosition;
                position.x += (i * blockWidth) + (i * gapSize);
                blocks[i].transform.position = position;
                blocks[i].transform.rotation = Quaternion.identity;

                // Get a valid color for the block and remove it from the available list.
                Color blockColor = GetAvailableColor(ref availableColors);
                blocks[i].GetComponent<SpriteRenderer>().color = blockColor;

                // Renable the block if it doesn't match the current background color.
                if (!CheckColorAgainstBackground(blockColor))
                {
                    blocks[i].SetActive(true);
                }
                else if (i == enabledBlockIndex)
                {
                    // Adjusts the reference block for recycling the block line.
                    IncrementValue(ref enabledBlockIndex, numberOfBlocks - 1);
                    solidLine = false;
                }
                else
                {
                    solidLine = false;
                }

                // Ensure the block's movement speed is reset to avoid blocks spawning slow due to late change changes.
                blocks[i].GetComponent<BlockMovement>().SetMovementSpeed(startingBlockSpeed);
            }
        }

        // Reset the x spawn coordinate for the next time the function is called.
        spawnPosition.x -= gapSize + (blockWidth * 0.5f);

        if (colorManager.GetBackgroundChanged())
        {
            // Set it so that it is possible for the background color to change again.
            colorManager.SetBackgroundChanged(false);
        }

        if (gameManager.GetDifficultyIncremented())
        {
            // Set is so that the difficulty can be increased after the next set of lines.
            gameManager.SetDifficultyIncremented(false);
        }
    }

    /* Returns a valid color from a list of available colors and updates the list. */
    private Color GetAvailableColor(ref List<Color> colors)
    {
        Color color = colors[Random.Range(0, colors.Count)];
        colors.Remove(color);
        return color;
    }

    /* Disables the given block and updates the block enabled index if necessary. */
    private void DisableBlock(int blockIndex)
    {
        blocks[blockIndex].SetActive(false);
        if (blockIndex == enabledBlockIndex)
        {
            // Adjusts the reference block for recycling the block line.
            IncrementValue(ref enabledBlockIndex, numberOfBlocks - 1);

            // Check that the new enabled block reference isn't also disabled.
            while (blocks[enabledBlockIndex] && !blocks[enabledBlockIndex].activeInHierarchy)
            {
                IncrementValue(ref enabledBlockIndex, numberOfBlocks - 1);
            }
        }
    }

    /* Returns true is the given color matches the background color. */
    private bool CheckColorAgainstBackground(Color blockColor)
    {
        if (blockColor == backgroundRenderer.material.color)
        {
            return true;
        }
        return false;
    }

    /* Increments the given value accounting for wrap around. */
    private void IncrementValue(ref int value, int maxValue)
    {
        if (value == maxValue)
        {
            value = 0;
        }
        else
        {
            value++;
        }
    }

    /* Slows the blocks for a short time. */
    private IEnumerator SlowBlocks()
    {
        // Slows the blocks movement.
        startingBlockSpeed = blocks[0].GetComponent<BlockMovement>().GetMovementSpeed();
        for (int i = 0; i < numberOfBlocks; ++i)
        {
            blocks[i].GetComponent<BlockMovement>().SetMovementSpeed(startingBlockSpeed * 0.5f);
        }

        yield return new WaitForSeconds(1.0f);

        // Returns the blocks movement to normal speed.
        for (int i = 0; i < numberOfBlocks; ++i)
        {
            BlockMovement blockMovement = blocks[i].GetComponent<BlockMovement>();
            if (blockMovement.GetMovementSpeed() != startingBlockSpeed)
            {
                blockMovement.SetMovementSpeed(startingBlockSpeed);
            }
        }
    }
}
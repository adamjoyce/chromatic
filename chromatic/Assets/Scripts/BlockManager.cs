using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject blockPrefab;          // The block that will make up each line.
    public GameObject colorManager;         // The color manager in the scene.
    public Renderer backgroundRenderer;     // The background's renderer component - used for block spacing and start position.
    public int numberOfBlocks = 5;          // The number of blocks each line is made up of.

    private GameObject[] blocks;            // The array of blocks.
    private Vector3 spawnPosition;          // The starting spawn location for the line of blocks.
    private float despawnHeight;            // The height at which the block line is off the screen and can be recycled.
    private int enabledBlockIndex = 0;      // The index of a block in the line that is currently enabled, i.e. not the background colour.

    /* Use this for initialization. */
    private void Start()
    {
        // Grab the color manager if it is not assigned in the editor.
        if (!colorManager) { colorManager = GameObject.Find("ColorManager"); }

        blocks = new GameObject[numberOfBlocks];

        // X screen position for the bottom left of the background element.
        float backgroundExtentX = Camera.main.WorldToScreenPoint(new Vector3(backgroundRenderer.bounds.min.x, 0, 0)).x;
        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(backgroundExtentX, -1, 10));
        spawnPosition.y -= blockPrefab.GetComponent<Renderer>().bounds.size.y * 0.5f;

        despawnHeight = backgroundRenderer.bounds.size.y * 0.5f;

        SpawnBlocks();
    }

    /* Use this for initilization. */
    private void Update()
    {
        if (blocks[enabledBlockIndex].transform.position.y >= (despawnHeight * 0.5f))
        {
            SpawnBlocks();
        }
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
        List<Color> availableColors = new List<Color>(colorManager.GetComponent<ColorManager>().GetColors());

        if (!blocks[0])
        {
            // First time spawning the blocks.
            for (int i = 0; i < numberOfBlocks; ++i)
            {
                // Adjust the spawn position based on which block is being spawned in the line.
                Vector3 position = spawnPosition;
                position.x += (i * blockWidth) + (i * gapSize);

                // Get a valid color for the block and remove it from the available list.
                Color blockColor = availableColors[Random.Range(0, availableColors.Count - 1)];
                availableColors.Remove(blockColor);

                // Create the block.
                GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity);
                newBlock.GetComponent<SpriteRenderer>().color = blockColor;
                blocks[i] = newBlock;
            }
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
                Color blockColor = availableColors[Random.Range(0, availableColors.Count - 1)];
                availableColors.Remove(blockColor);

                blocks[i].GetComponent<SpriteRenderer>().color = blockColor;
            }

            // Renabled the block's movement.
            for (int i = 0; i < numberOfBlocks; ++i)
            {
                blocks[i].SetActive(true);
            }
        }

        // Reset the x spawn coordinate for the next time the function is called.
        spawnPosition.x -= gapSize + (blockWidth * 0.5f);
    }
}
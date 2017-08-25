using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject blockPrefab;          // The block that will make up each line.
    public Renderer backgroundRenderer;     // The background's renderer component - used for block spacing and start position.
    public int numberOfBlocks = 5;          // The number of blocks each line is made up of.

    private GameObject[] blocks;            // The array of blocks.
    //private GameObject blockLine;           // Parent gameobject to move and reset the line position uniformly.
    private Vector3 spawnPosition;          // The starting spawn location for the line of blocks.
    private float despawnHeight;            // The height at which the block line is off the screen and can be recycled.
    private int enabledBlockIndex = 0;      // The index of a block in the line that is currently enabled, i.e. not the background colour.

    /* Use this for initialization. */
    private void Start()
    {
        blocks = new GameObject[numberOfBlocks];

        // Set up the block line component.
        //blockLine = new GameObject("BlockLine");
        //blockLine.SetActive(false);
        //blockLine.AddComponent<Rigidbody2D>();
        //blockLine.GetComponent<Rigidbody2D>().gravityScale = 0;
        //blockLine.AddComponent<BlockMovement>();

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

        // Set the line parent object to be at the same height as the blocks.
        //blockLine.transform.position = spawnPosition;

        // Position the first block away from the border.
        spawnPosition.x += gapSize + (blockWidth * 0.5f);

        if (!blocks[0])
        {
            // First time spawning the blocks.
            for (int i = 0; i < numberOfBlocks; ++i)
            {
                // Adjust the spawn position based on which block is being spawned in the line.
                Vector3 position = spawnPosition;
                position.x += (i * blockWidth) + (i * gapSize);

                GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity);
                //newBlock.transform.SetParent(blockLine.transform);
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
            }

            // Renabled the block's movement.
            for (int i = 0; i < numberOfBlocks; ++i)
            {
                blocks[i].SetActive(true);
            }
        }

        // Reset the x spawn coordinate for the next time the function is called.
        spawnPosition.x -= gapSize + (blockWidth * 0.5f);

        //blockLine.SetActive(true);
    }
}
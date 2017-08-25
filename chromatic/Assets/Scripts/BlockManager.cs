using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject blockPrefab;          // The block that will make up each line.
    public Renderer backgroundRenderer;     // The background's renderer component - used for block spacing and start position.
    public int numberOfBlocks = 5;          // The number of blocks each line is made up of.

    private GameObject[] blocks;            // The array of blocks.
    private Vector3 spawnPosition;          // The starting spawn location for the line of blocks.

    /* Use this for initialization. */
    void Start()
    {
        blocks = new GameObject[numberOfBlocks];

        // X screen position for the bottom left of the background element.
        float backgroundExtentX = Camera.main.WorldToScreenPoint(new Vector3(backgroundRenderer.bounds.min.x, 0, 0)).x;
        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(backgroundExtentX, -1, 10));

        // Account for the block width.
        //float blockWidth = blockPrefab.GetComponent<Renderer>().bounds.size.x;
        //spawnPosition.x += blockWidth * 0.5f;

        SpawnBlocks();
    }

    /* Spawns a line of blocks. */
    private void SpawnBlocks()
    {
        // Calculate the values needed for the block spacing and positions.
        float blockWidth = blockPrefab.GetComponent<Renderer>().bounds.size.x;
        float gapSize = ((backgroundRenderer.bounds.size.x - (blockWidth * numberOfBlocks)) / (numberOfBlocks + 1));

        // Position the first block away from the border.
        spawnPosition.x += gapSize + (blockWidth * 0.5f);

        for (int i = 0; i < numberOfBlocks; ++i)
        {
            // Adjust the spawn position based on which block is being spawned in the line.
            Vector3 position = spawnPosition;
            position.x += (i * blockWidth) + (i * gapSize);

            Instantiate(blockPrefab, position, Quaternion.identity);
        }

        // Reset the x spawn coordinate for the next time the function is called.
        spawnPosition.x -= gapSize;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject blockPrefab;          // The block that will make up each line.
    public GameObject background;           // The background used to determine spacing between blocks in the line.
    public GameObject spawnPosition;        // The starting spawn location for the line of blocks.
    public int numberOfBlocks = 5;          // The number of blocks each line is made up of.

    private GameObject[] blocks;            // The array of blocks.

    /* Use this for initialization. */
    void Start()
    {
        blocks = new GameObject[numberOfBlocks];
        SpawnBlocks();
    }

    /* Update is called once per frame. */
    void Update()
    {

    }

    /* Spawns a line of blocks. */
    private void SpawnBlocks()
    {
        float blockSizeX = blockPrefab.GetComponent<Renderer>().bounds.size.x;
        float gapSize = ((background.GetComponent<Renderer>().bounds.size.x - (blockSizeX * numberOfBlocks)) / numberOfBlocks);
        for (int i = 0; i < numberOfBlocks; ++i)
        {
            // Adjust the spawn position based on which block is being spawned in the line.
            Vector3 position = spawnPosition.transform.position;
            position.x += (i * blockSizeX) + (i * gapSize);

            Instantiate(blockPrefab, position, Quaternion.identity);
        }
    }
}
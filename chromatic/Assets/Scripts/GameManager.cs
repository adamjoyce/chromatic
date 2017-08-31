using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public BlockManager blockManager;               // The scene's block manager script used for tracking the number of block lines.
    public int lineDifficultyIncrement = 5;         // The number of lines that must be passed before the difficulty increases.
    public float difficultyMultiplier = 1.1f;       // The amount by with the difficulty increases every set number of lines.

    private int lineScore = 1;                      // The number of block lines the player has successfully traversed.
    private bool difficultyIncremented = false;     // True if the difficult has been increased for the current block line.

	/* Use this for initialization. */
	private void Start() 
	{
		if (!blockManager) { blockManager = GameObject.Find("BlockManager").GetComponent<BlockManager>(); }
	}
	
	/* Update is called once per frame. */
	private void Update() 
	{
		if (!difficultyIncremented && (lineScore % lineDifficultyIncrement) == 0)
        {
            difficultyIncremented = true;
            IncreaseDifficulty();
        }
	}

    /* Increases the difficulty by the difficulty multiplier. */
    public void IncreaseDifficulty()
    {
        blockManager.UpdateLinesMovementSpeed(difficultyMultiplier);
    }

    /* Get the current line score. */
    public int GetLineScore()
    {
        return lineScore;
    }

    /* Sets the new line score. */
    public void SetLineScore(int newScore)
    {
        lineScore = newScore;
    }

    /* Returns true if the difficulty has already been incremented for this set of block lines. */
    public bool GetDifficultyIncremented()
    {
        return difficultyIncremented;
    }

    /* Sets the flag indicating the difficulty has been increased for the current set of block lines. */
    public void SetDifficultyIncremented(bool isIncremented)
    {
        difficultyIncremented = isIncremented;
    }
}

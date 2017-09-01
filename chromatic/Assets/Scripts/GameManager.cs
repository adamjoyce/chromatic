﻿using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public BlockManager blockManager;                       // The scene's block manager script used for tracking the number of block lines.
    public int lineDifficultyIncrement = 5;                 // The number of lines that must be passed before the difficulty increases.
    public float difficultyMultiplier = 1.1f;               // The amount by with the difficulty increases every set number of lines.

    public Image difficultyImage;                           // The image that flashes when the difficulty increases.
    public Color difficultyImageColor = Color.white;        // The color of the difficulty image.
    public float flashSpeed = 5.0f;                         // The speed at which the difficulty image will fade.

    private int lineScore = 0;                              // The number of block lines the player has successfully traversed.
    private bool difficultyIncremented = false;             // True if the difficult has been increased for the current block line.
    private bool isPlaying = false;                         // True when the player begins the game by selecting 'PLAY'.

	/* Use this for initialization. */
	private void Start() 
	{
		if (!blockManager) { blockManager = FindObjectOfType<BlockManager>(); }
        if (!difficultyImage) { difficultyImage = GameObject.Find("DifficultyFlash").GetComponent<Image>(); }

        // Start with a clear screen.
        difficultyImage.color = Color.clear;
	}
	
	/* Update is called once per frame. */
	private void Update() 
	{
        if (isPlaying)
        {
            // Difficulty scaling.
            if (!difficultyIncremented && lineScore != 0 && (lineScore % lineDifficultyIncrement) == 0)
            {
                difficultyIncremented = true;
                IncreaseDifficulty();
            }

            // Difficulty UI fading.
            if (difficultyImage.color != Color.clear)
            {
                difficultyImage.color = Color.Lerp(difficultyImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
        }
	}

    /* Increases the difficulty by the difficulty multiplier. */
    public void IncreaseDifficulty()
    {
        blockManager.UpdateLinesMovementSpeed(difficultyMultiplier);
        difficultyImage.color = difficultyImageColor;
    }

    /* Return the current line score. */
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

    /* Return true if the game is currently in progress. */
    public bool GetIsPlaying()
    {
        return isPlaying;
    }

    /* Sets whether or not the game is in progress. */
    public void SetIsPlaying(bool playing)
    {
        isPlaying = playing;
    }

    /* Quits the game / editor. */
    public void QuitGame()
    {
        Application.Quit();

        // Must be commented out when building.
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
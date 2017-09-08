using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public Transform player;                                // The player character's transform.
    public BlockManager blockManager;                       // The scene's block manager script used for tracking the number of block lines.
    public UIManager UIManager;                             // The scene's ui manager script used for updating and resetting the score text.
    public AudioManager audioManager;                       // The game's audio manager script for playing non-position dependent sound clips.
    public int lineDifficultyIncrement = 5;                 // The number of lines that must be passed before the difficulty increases.
    public float difficultyMultiplier = 1.1f;               // The amount by with the difficulty increases every set number of lines.

    public Image difficultyImage;                           // The image that flashes when the difficulty increases.
    public Color difficultyImageColor = Color.white;        // The color of the difficulty image.
    public float flashSpeed = 5.0f;                         // The speed at which the difficulty image will fade.

    public float gameOverSlowness = 10.0f;                  // The slow down that happens when the player loses.  

    private int lineScore = 0;                              // The number of block lines the player has successfully traversed.
    private bool difficultyIncremented = false;             // True if the difficult has been increased for the current block line.
    private bool isPlaying = false;                         // True when the player begins the game by selecting 'PLAY'.
    private bool gameResetting = false;                     // True when the game has just reset.

    /* PROPERTIES. */
    public int LineScore
    {
        get { return lineScore; }
        set {
            lineScore = value;
            UIManager.UpdateScore(lineScore);
        }
    }

    public bool DifficultyIncremented { get; set; }
    public bool IsPlaying { get; set; }
    public bool GameResetting { get; set; }
    /* END OF PROPERTIES. */

	/* Use this for initialization. */
	private void Start() 
	{
        if (!player) { player = GameObject.Find("Player").transform; }
		if (!blockManager) { blockManager = FindObjectOfType<BlockManager>(); }
        if (!UIManager) { UIManager = FindObjectOfType<UIManager>(); }
        if (!audioManager) { audioManager = FindObjectOfType<AudioManager>(); }
        if (!difficultyImage) { difficultyImage = GameObject.Find("DifficultyFlash").GetComponent<Image>(); }

        // Start with a clear screen.
        difficultyImage.color = Color.clear;
	}
	
	/* Update is called once per frame. */
	private void Update() 
	{
        if (IsPlaying)
        {
            // Difficulty scaling.
            if (!difficultyIncremented && lineScore != 0 && (lineScore % lineDifficultyIncrement) == 0)
            {
                difficultyIncremented = true;
                IncreaseDifficulty();
            }
        }

        // Difficulty UI fading.
        if (difficultyImage.color != Color.clear)
        {
            // Fade color back to transparent for flash effect - note fade is pseudo-linear due to dynamic starting point.
            difficultyImage.color = Color.Lerp(difficultyImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    /* Increases the difficulty by the difficulty multiplier. */
    public void IncreaseDifficulty()
    {
        blockManager.UpdateLinesMovementSpeed(difficultyMultiplier);
        difficultyImage.color = difficultyImageColor;
        audioManager.Play("DifficultyIncrement");
    }

    /* Quits the game / editor. */
    public void QuitGame()
    {
        Application.Quit();

        // Must be commented out when building.
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    /* Ends and resets the game. */
    public IEnumerator EndAndResetGame()
    {
        // Player death sound.
        audioManager.Play("PlayerDeath");

        // Slow down game time.
        Time.timeScale = 1.0f / gameOverSlowness;
        Time.fixedDeltaTime = Time.fixedDeltaTime / gameOverSlowness;

        yield return new WaitForSeconds(1.0f / gameOverSlowness);

        // Hide block line, reset player, activate menu.
        blockManager.ResetBlockLine();
        player.position = new Vector2(0, player.position.y);
        UIManager.SetMenu(true);

        // Reset game time.
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * gameOverSlowness;

        GameResetting = true;
        IsPlaying = false;
    }
}
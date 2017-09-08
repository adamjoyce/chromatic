using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    public Transform player;                        // The player's transform for the player height.
    public GameManager gameManager;                 // The scene's game manager used after a menu option is selected.
    public Renderer backgroundRenderer;             // The background renderer for the min and max bound positions.
    public Text playText;                           // The play text UI element.
    public Text quitText;                           // The quit text UI element.
    public Text scoreText;                          // The score text UI element.
    public Color textOriginalColor = Color.white;   // The original color for a UI text element.
    public Color textFlashColor = Color.black;      // The color a UI text element flashes when selected.
    public float fadeSpeed = 2.0f;                  // The speed at which the UI text element fades.

    private bool playSelected = false;              // True when the play menu option is selected.
    private bool quitSelected = false;              // True when the quit menu option is selected.

    private float flashStartTime = 0.0f;            // The game time when the selection flash begins.

	/* Use this for initialization. */
	private void Start() 
	{
		if (!player) { player = GameObject.Find("Player").transform; }
        if (!gameManager) { gameManager = FindObjectOfType<GameManager>(); }
        if (!backgroundRenderer) { backgroundRenderer = GameObject.Find("Background").GetComponent<Renderer>(); }
        if (!playText) { playText = GameObject.Find("PlayText").GetComponent<Text>(); }
        if (!quitText) { quitText = GameObject.Find("QuitText").GetComponent<Text>(); }
        if (!scoreText) { scoreText = GameObject.Find("ScoreText").GetComponent<Text>(); }

        // Setup the menu text element positions.
        Vector3 minBackgroundBound = backgroundRenderer.bounds.min;
        Vector3 maxBackgroundBound = backgroundRenderer.bounds.max;
        playText.transform.position = new Vector3(minBackgroundBound.x, player.position.y, 0);
        quitText.transform.position = new Vector3(maxBackgroundBound.x, player.position.y, 0);

        // Set the menu text elements starting colors.
        playText.color = textOriginalColor;
        quitText.color = textOriginalColor;

        // Set the score text rectangle size to match the background size.
        float xMin = Camera.main.WorldToScreenPoint(minBackgroundBound).x;
        float xMax = Camera.main.WorldToScreenPoint(maxBackgroundBound).x;
        scoreText.rectTransform.sizeDelta = new Vector2(xMax - xMin, scoreText.rectTransform.sizeDelta.y) / GetComponentInParent<Canvas>().scaleFactor;
    }

    /* Update is called once per frame. */
    private void Update()
    {
        if (!gameManager.IsPlaying)
        {
            if (playSelected)
            {
                // Call the 'PLAY' menu option's behaviour.
                StartCoroutine(ActivateBehaviourAfterAnimation(playText));
                playSelected = false;
            }
            else if (quitSelected)
            {
                // Call the 'QUIT' menu option's behaviour.
                StartCoroutine(ActivateBehaviourAfterAnimation(quitText));
                quitSelected = false;
            }

            // Linearly fade text colors back to white.
            if (playText.color != Color.white)
            {
                playText.color = Color.Lerp(textFlashColor, textOriginalColor, (Time.time - flashStartTime) / fadeSpeed);
            }

            if (quitText.color != Color.white)
            {
                quitText.color = Color.Lerp(textFlashColor, textOriginalColor, (Time.time - flashStartTime) / fadeSpeed);
            }
        }
    }

    /* Determines which menu option was selected and flags Update to call it's related behaviour. */
    public void ActivateMenuOption(Transform selectedWall)
    {
        // Determine which option was selected based on which wall the player collided with.
        if (selectedWall.position.x < 0)
        {
            // Left option selected - 'PLAY'.
            playSelected = true;
            playText.color = textFlashColor;
        }
        else
        {
            // Right option selected - 'QUIT'.
            quitSelected = true;
            quitText.color = textFlashColor;
        }
        flashStartTime = Time.time;
    }

    /* Updates the score text UI element. */
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    /* Enable/Disable menu display. */
    public void SetMenu(bool isActive)
    {
        playText.gameObject.SetActive(isActive);
        quitText.gameObject.SetActive(isActive);
    }

    /* Animates the given UI text element with a flash. */
    private IEnumerator ActivateBehaviourAfterAnimation(Text text)
    {
        FindObjectOfType<AudioManager>().Play("MenuSelection");
        yield return new WaitForSeconds(fadeSpeed);
        if (text == playText)
        {
            Play();
        }
        else if (text == quitText)
        {
            Quit();
        }
    }

    /* The beahviour called when the 'PLAY' menu option is selected. */
    private void Play()
    {
        SetMenu(false);
        gameManager.IsPlaying = true;

        // Reset the game score.
        gameManager.LineScore = 0;
    }

    /* The beahviour called when the 'QUIT' menu option is selected. */
    private void Quit()
    {
        gameManager.QuitGame();
    }
}

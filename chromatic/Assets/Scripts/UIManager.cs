using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    public Transform player;                    // The player's transform for the player height.
    public Renderer backgroundRenderer;         // The background renderer for the min and max bound positions.
    public Text playText;                       // The play text UI element.
    public Text quitText;                       // The quit text UI element.

    private bool animationFinished = false;     // True once the UI menu selection animation has completed.

	/* Use this for initialization. */
	private void Start() 
	{
		if (!player) { player = GameObject.Find("Player").transform; }
        if (!backgroundRenderer) { backgroundRenderer = GameObject.Find("Background").GetComponent<Renderer>(); }
        if (!playText) { playText = GameObject.Find("PlayText").GetComponent<Text>(); }
        if (!quitText) { quitText = GameObject.Find("QuitText").GetComponent<Text>(); }

        // Setup the text element positions.
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(player.position);
        Vector3 minBackgroundBound = Camera.main.WorldToScreenPoint(backgroundRenderer.bounds.min);
        Vector3 maxBackgroundBound = Camera.main.WorldToScreenPoint(backgroundRenderer.bounds.max);
        playText.transform.position = new Vector3(minBackgroundBound.x, playerScreenPosition.y);
        quitText.transform.position = new Vector3(maxBackgroundBound.x, playerScreenPosition.y);
    }

    /* Determine menu selection based on collided gameobject. */
    public string DetermineMenuSelection(GameObject selectedWall)
    {
        string selectedMenuOption = "";
        if ((playText.transform.position.x) - (selectedWall.transform.position.x) < (quitText.transform.position.x - selectedWall.transform.position.x))
        {
            // Play is the selection.
            selectedMenuOption = playText.gameObject.name;
        }
        else
        {
            selectedMenuOption = quitText.gameObject.name;
        }
        return selectedMenuOption.Substring(0, selectedMenuOption.Length - 4);
    }

    /* Flash the selected menu option. */
    public IEnumerator AnimateSelection(string selectedOption)
    {
        Text selectionToAnimate;
        if (selectedOption == "Play")
        {
            selectionToAnimate = playText;
        }
        else
        {
            selectionToAnimate = quitText;
        }

        Color flashColor = Color.black;
        selectionToAnimate.color = flashColor;
        selectionToAnimate.color = Color.Lerp(selectionToAnimate.color, Color.white, 2.0f * Time.deltaTime);

        yield return new WaitUntil(() => selectionToAnimate.color == Color.white);
        animationFinished = true;
    }

    /* Returns true when the selection animation has finished. */
    public bool GetAnimationFinished()
    {
        return animationFinished;
    }

    /* Sets the state of the selection animation. */
    public void SetAnimationFinished(bool finished)
    {
        animationFinished = finished;
    }

    /* Sets the the activity of the menu elements. */
    public void SetMenu(bool isActive)
    {
        playText.gameObject.SetActive(isActive);
        quitText.gameObject.SetActive(isActive);
    }
}

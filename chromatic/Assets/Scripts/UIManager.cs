using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    public Transform player;                    // The player's transform for the player height.
    public Renderer backgroundRenderer;         // The background renderer for the min and max bound positions.
    public Text playText;                       // The play text UI element.
    public Text quitText;                       // The quit text UI element.              

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
	
	/* Update is called once per frame. */
	private void Update() 
	{
		
	}
}

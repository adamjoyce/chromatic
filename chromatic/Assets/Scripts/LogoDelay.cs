using System.Collections;
using UnityEngine;

public class LogoDelay : MonoBehaviour 
{
    public float logoDelay = 3.0f;          // How long the logo will be displayed before switching scenes.

	/* Use this for initialization. */
	private void Start() 
	{
        Cursor.visible = false;
        StartCoroutine(WaitAndLoadScene("Game"));
	}

    /* Waits a short time before loading the main scene. */
    private IEnumerator WaitAndLoadScene(string scene)
    {
        SceneController sceneController = FindObjectOfType<SceneController>();
        yield return new WaitForSeconds(logoDelay);
        sceneController.FadeAndLoadScene(scene);
        SceneController.AfterSceneLoad += PlayBackgroundAudio;
    }

    /* Starts playing the background audio after the game scene is loaded. */
    private void PlayBackgroundAudio()
    {
        FindObjectOfType<AudioManager>().Play("Background");
        SceneController.AfterSceneLoad -= PlayBackgroundAudio;
    }
}

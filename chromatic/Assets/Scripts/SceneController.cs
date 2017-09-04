using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour 
{
    public static event Action BeforeSceneUnload;           // Event delegate that is called just before a scene is unloaded.
    public static event Action AfterSceneLoad;              // Event delegate that is called just after a scene is loaded.

    public CanvasGroup faderCanvasGroup;                    // The canvas group that controls the Image used for scene fades.
    public float fadeDuration = 1.0f;                       // How long it will take for the scene to fade to or from black.
    public string initialScene = "StartingScene";           // The first scene to be loaded from the persistent scene.

    private bool isFading = false;                          // Whether the scene is currently fading to or from black.

    /* Use this for initialization. */
    private IEnumerator Start()
    {
        // Set the initial alpha to start with a black scene.
        faderCanvasGroup.alpha = 1.0f;

        // Start loading the first scene and wait for it to finish.
        yield return StartCoroutine(LoadSceneAndSetActive(initialScene));

        // Fade the scene in once it has finished loading.
        StartCoroutine(Fade(0.0f));
    }

    /* The external point of contact for scene transitions. */
    public void FadeAndLoadScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }

    /* Returns the currently active scene build index. */
    public int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    /* Handles a complete scene transition including any actions immediately before or after the change occurs. */
    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        // Fade to black and wait for it to complete before continuing.
        yield return StartCoroutine(Fade(1.0f));

        // Resolve any subscribed actions necessary before the current scene is unloaded.
        if (BeforeSceneUnload != null)
        {
            BeforeSceneUnload();
        }

        // Unloads the active scene.
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Load the new scene and wait for it to complete before continuing.
        yield return LoadSceneAndSetActive(sceneName);

        // Resolve any subscribed actions necessary immediately after a new scene is loaded.
        if (AfterSceneLoad != null)
        {
            AfterSceneLoad();
        }

        // Fade the scene in and wait for it to complete before exiting the function.
        yield return StartCoroutine(Fade(0.0f));
    }

    /* Loads the given scene and sets it to be the new active scene within the SceneManager. */
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        // Allow the scene to load over several frames and add it to the already loaded scenes (just the Persistent scene to start with).
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Set the newly loaded scene as the active scene indicating that it should be unloaded next.
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    /* Handles the fade effect between scene transitions. */
    private IEnumerator Fade(float finalAlpha)
    {
        // Stops attempts to fade and switch the scenes again.
        isFading = true;

        // Halts input into the old scene.
        faderCanvasGroup.blocksRaycasts = true;

        // Fade between the current and final alpha values.
        float fadeSpeed = Mathf.Abs((faderCanvasGroup.alpha - finalAlpha) / fadeDuration);
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            // Work the canvs group alpha towards the final alpha.
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            // Wait for a frame then continue...
            yield return null;
        }

        // Enable attempts to fade and switch to new scenes.
        isFading = false;

        // Enable input on the new scene.
        faderCanvasGroup.blocksRaycasts = false;
    }
}

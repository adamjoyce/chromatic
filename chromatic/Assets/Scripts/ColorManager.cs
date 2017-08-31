using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour 
{
    public Color[] colors;                      // The list of colors the blocks or background can take.
    public GameObject background;               // The scene background for changing the background color.

    private bool backgroundChanged = false;     // Indicates the background has changed once already this block line.

    /* Use this for initialization. */
    private void Start() 
	{
        if (!background) { background = GameObject.Find("Background"); }
	}

    /* Color array access for external scripts. */
    public Color[] GetColors()
    {
        return colors;
    }

    /* Changes and returns the background's color. */
    public Color ChangeBackgroundColor()
    {
        // Remove the current background color from the choice of new colors.
        List<Color> backgroundColors = new List<Color>(colors);
        Material backgroundMaterial = background.GetComponent<Renderer>().material;
        backgroundColors.Remove(backgroundMaterial.color);

        // Randomly select a new background color.
        Color newColor = backgroundColors[Random.Range(0, backgroundColors.Count)];
        backgroundMaterial.color = newColor;
        Camera.main.backgroundColor = newColor * 0.5f;

        return newColor;
    }

    /* Returns if the background has been changed this block line. */
    public bool GetBackgroundChanged()
    {
        return backgroundChanged;
    }

    /* Sets if the background has been changed this block line. */
    public void SetBackgroundChanged(bool changed)
    {
        backgroundChanged = changed;
    }
}

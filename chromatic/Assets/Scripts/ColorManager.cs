using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour 
{
    public Color[] colors;              // The list of colors the blocks or background can take.
    public GameObject background;       // The scene background for changing the background color.

	/* Use this for initialization. */
	private void Start() 
	{
        if (!background) { background = GameObject.Find("Background"); }
	}
	
	/* Update is called once per frame. */
	private void Update() 
	{
		
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
}

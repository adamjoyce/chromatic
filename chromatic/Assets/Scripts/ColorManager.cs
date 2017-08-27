using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour 
{
    public Color[] colors;          // The list of colors the blocks or background can take.

	/* Use this for initialization. */
	private void Start() 
	{

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
}

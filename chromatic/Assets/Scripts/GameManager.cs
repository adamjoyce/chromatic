using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public BlockManager blockManager;           // The scene's block manager script used for tracking the number of block lines.

	/* Use this for initialization. */
	private void Start() 
	{
		if (!blockManager) { blockManager = GameObject.Find("BlockManager").GetComponent<BlockManager>(); }
	}
	
	/* Update is called once per frame. */
	private void Update() 
	{
		
	}
}

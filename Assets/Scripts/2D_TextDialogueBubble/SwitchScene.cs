/**
*   See Assets -> 2D Dialog Bubble
**/
using UnityEngine;
using System.Collections;

public class SwitchScene : MonoBehaviour 
{
	public void Switch(string vScene)
	{
		Application.LoadLevel (vScene); //change scene
	}
}

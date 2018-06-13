/**
*   Filename: SlimeSpawner.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script enables the possibility to spawn a slime upon interacting.
*   
**/
using UnityEngine;


public class SlimeSpawner : MonoBehaviour {

	public GameObject _Slimey;

	public void activateSlime()
	{
		_Slimey.SetActive (true);
	}



}
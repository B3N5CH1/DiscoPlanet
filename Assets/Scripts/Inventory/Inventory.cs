/**
*   Filename: Inventory.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This class handles the Inventory of the player.
*       This is done by saving the collected items in the PlayerPrefs
*   
**/
using UnityEngine;


public class Inventory {

	public bool addItem(string name) {
		if (checkItem(name) == 0) {
			PlayerPrefs.SetInt (name, 1);
            return true;
		} else {
            return false;
		}
	}

	public int checkItem(string name) {
		return PlayerPrefs.GetInt (name, 0);
	}

}
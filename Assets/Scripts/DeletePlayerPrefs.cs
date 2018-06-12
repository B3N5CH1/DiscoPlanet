using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class handle the removale of the player preferences, which are used as our data storage.
 */

public class DeletePlayerPrefs : MonoBehaviour {

    // Simple constructor
	public DeletePlayerPrefs ()
    {

    }

    // Method that delete the game progression preferences (not the settings)
    public void DeleteAll()
    {
        PlayerPrefs.DeleteKey("level1CheckPoint");
        PlayerPrefs.DeleteKey("level1Completed");
        PlayerPrefs.DeleteKey("level2Completed");
		PlayerPrefs.DeleteKey("progressionStarted");
        PlayerPrefs.DeleteKey("firstTimeLvl1");

		PlayerPrefs.DeleteKey ("Shiny Rock");
		PlayerPrefs.DeleteKey ("Chest");
		PlayerPrefs.DeleteKey ("Light Graviton Collector");
		PlayerPrefs.DeleteKey ("Light Gravitons");
		PlayerPrefs.DeleteKey ("Ice Cream");
    }
}

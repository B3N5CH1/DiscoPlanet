using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayerPrefs : MonoBehaviour {

	public DeletePlayerPrefs ()
    {

    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteKey("level1Completed");
        PlayerPrefs.DeleteKey("level2Completed");
        PlayerPrefs.DeleteKey("progressionStarted");
    }
}

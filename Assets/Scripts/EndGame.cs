using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This script handle what happens after the spachips animation ends.
 */

public class EndGame : MonoBehaviour {

    // Go to the end scene
	public void GoToEndScene()
    {
        SceneManager.LoadScene(6);
    }
}

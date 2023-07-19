using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        // Loads the next scene in the build order
        Debug.Log("Loading Game...");
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        // Quits the game
        Debug.Log("Quiting Game...");
        Application.Quit();
    }
}

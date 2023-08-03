using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public AudioClip buttonClickSound;
    public void PlayGame()
    {
        // Loads the next scene in the build order
        AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position, 1f);
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        // Loads the next scene in the build order
        Debug.Log("Loading Game...");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(1);
    }
}

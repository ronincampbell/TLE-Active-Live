using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject finishMenuUI;
    public TextMeshProUGUI finishTime;
    public TextMeshProUGUI loopNumText;
    public Camera gameCam;
    public GameObject GameAudio;
    private bool pressToContinue = false;

    private void Update(){
        if (pressToContinue && Input.anyKeyDown){
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && gameManager.playerIsDead == false) {
            finishMenuUI.SetActive(true);
            finishTime.text = gameManager.countdownText.text;
            loopNumText.text = "LOOP #" + gameManager.loopNum.ToString();
            gameCam.GetComponent<ChromaticAberration>().enabled = true;
            GameAudio.GetComponent<AudioLowPassFilter>().enabled = true;
            Time.timeScale = 0f;
            pressToContinue = true;
        }
    }
}

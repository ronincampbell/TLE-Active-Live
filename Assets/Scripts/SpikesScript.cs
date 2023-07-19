using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SpikesScript : MonoBehaviour
{
    public GameObject GameManager;
    private GameManager countdownTimer;
    public bool spikesUp;
    public bool resetNow;
    // Update is called once per frame
    private void Start() {
        spikesUp = true;
    }
    void Update()
    {
        checkForReset();

        if (spikesUp){
            this.GetComponent<Animator>().SetBool("spikesUp", true);
        } else {
            this.GetComponent<Animator>().SetBool("spikesUp", false);
        }
    }
    
    private void checkForReset(){
        if (resetNow)
        {
            Debug.Log("Spikes have been reset");
            spikesUp = true;
            resetNow = false;
            this.GetComponent<Animator>().SetTrigger("forceActivate");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)        
    {
        if (other.gameObject.tag == "Player" && spikesUp && GameManager.GetComponent<GameManager>().playerIsDead == false)
        {
            GameManager.GetComponent<GameManager>().killPlayer();
        }
    }
}

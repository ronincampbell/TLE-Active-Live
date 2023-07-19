using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour
{
    public GameObject GameManager;
    public bool sawActive;
    public bool resetNow;
    private bool sawAlreadyActive = false;
    // Start is called before the first frame update
    void Start()
    {
        sawActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        checkForReset();

        if (sawActive && !sawAlreadyActive){
            //Debug.Log("Saw is active");
            this.GetComponent<Animator>().SetTrigger("activateSaw");
            this.GetComponent<Animator>().ResetTrigger("deactivateSaw");
            this.GetComponent<BoxCollider2D>().enabled = true;
            sawAlreadyActive = true;
        } else if (!sawActive) {
            //Debug.Log("Saw is inactive");
            this.GetComponent<Animator>().SetTrigger("deactivateSaw");
            this.GetComponent<Animator>().ResetTrigger("activateSaw");
            this.GetComponent<BoxCollider2D>().enabled = false;
            sawAlreadyActive = false;   
        }
    }

    private void checkForReset(){
        if (resetNow)
        {
            //Debug.Log("Saw has been reset");
            sawActive = true;
            resetNow = false;
            sawAlreadyActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)        
    {
        if (other.gameObject.tag == "Player" && sawActive && GameManager.GetComponent<GameManager>().playerIsDead == false)
        {
            GameManager.GetComponent<GameManager>().killPlayer();
        }
    }
}

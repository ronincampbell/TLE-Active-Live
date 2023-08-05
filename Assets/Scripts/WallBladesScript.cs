using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBladesScript : MonoBehaviour
{
    public GameObject GameManager;
    public bool bladesActive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!bladesActive){
            this.GetComponent<Animator>().SetBool("bladesActive", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && bladesActive)
        {
            //Debug.Log("Player hit wall blades");
            GameManager.GetComponent<GameManager>().killPlayer();
        }
    }

    // What the fuck is this code? I don't know why this is needed but animation breaks if it's removed. I think i was drunk when i wrote this.
    public void BladesActive(){
        this.GetComponent<BoxCollider2D>().enabled = true;
    }
    
    public void BladesInactive(){
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
}

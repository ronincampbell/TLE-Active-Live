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
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Player hit wall blades");
            GameManager.GetComponent<GameManager>().killPlayer();
        }
    }

    public void BladesActive(){
        this.GetComponent<BoxCollider2D>().enabled = true;
    }
    
    public void BladesInactive(){
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
}

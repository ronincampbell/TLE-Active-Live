using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamScript : MonoBehaviour
{
    public bool laserActive = true;
    public GameManager gameManager;
    // Update is called once per frame
    void Update()
    {
        if (!laserActive)
        {
            this.GetComponent<Animator>().SetBool("LaserDeactivated", true);
        } else
        {
            this.GetComponent<Animator>().SetBool("LaserDeactivated", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {    
        if (other.gameObject.tag == "Player" && laserActive)
        {
            gameManager.killPlayer();
        }
    }
}

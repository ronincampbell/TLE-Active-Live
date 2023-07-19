using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This code is awful and i cant be bothered to comment it so gl ig
public class LaserTurretScript : MonoBehaviour
{
    public float fireRate = 10f;
    private float fireTimer;
    private bool isFiring;
    public bool isActive;
    private bool playerIsInTrigger;
    private bool playerInLaser;
    private bool laserActive = false;
    private bool timerReady = false;
    public GameManager gameManager;
    // Start is called before the first rfame update
    void Start()
    {
        isActive = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        //Debug.Log("Fire timer: " + fireTimer);
        if (fireTimer >= fireRate && !isFiring && isActive && !timerReady)
        {
            Debug.Log("Attempting to Fire laser");
            this.GetComponent<Animator>().SetTrigger("FireLaser");
            isFiring = true;
            fireTimer = 0f;
            timerReady = true;
            this.GetComponent<Animator>().SetBool("TurretIsActive", true);
        }

        if (laserActive && playerIsInTrigger && isActive && isFiring){
            Debug.Log("Attempting to kill player");
            gameManager.killPlayer();
        }

        if (!isActive){
            this.GetComponent<Animator>().SetBool("TurretIsActive", false);
        }

        if (!timerReady){
            this.GetComponent<Animator>().SetBool("TurretIsActive", false);
        }
    }

    public void LaserActive(){
        laserActive = true;
    }

    public void LaserNotActive(){
        laserActive = false;
        isFiring = false;
        fireTimer = 0f;
    }

    public void TurretReset()
    {
        laserActive = false;
        isFiring = false;
        fireTimer = 0f;
        isActive = true;
        timerReady = false;
        this.GetComponent<Animator>().Play("laserTurretIdle");
        this.GetComponent<Animator>().SetBool("TurretIsActive", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collider triggered");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is in laser");
            playerIsInTrigger = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is not in laser");
            playerIsInTrigger = false;
        }
    }

    public void WaitForAnim(){
        timerReady = false;
    }

    
}

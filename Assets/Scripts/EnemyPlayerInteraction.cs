using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerInteraction : MonoBehaviour
{
    public GameObject respawnPoint;
    public EnemyDroidAI enemyDroidAI;
    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && enemyDroidAI.followEnabled == true){
            Debug.Log("Player hit");
            gameManager.killPlayer();
        }
    }

    public void activateEnemy(){
        this.gameObject.GetComponent<Animator>().SetBool("IsActivated", true);
        enemyDroidAI.followEnabled = true;
    }

    public void ResetDroid(){
        enemyDroidAI.followEnabled = false;
        this.gameObject.GetComponent<Animator>().SetBool("IsDead", true);
    }

    public void RespawnEnemy(){
        this.gameObject.GetComponent<Animator>().SetBool("IsDead", false);
        this.gameObject.transform.position = respawnPoint.transform.position;
        //StartCoroutine(waitForSec());
    }

    private IEnumerator waitForSec(){
        yield return new WaitForSeconds(1.5f);
        allowMovement();
    }

    private void allowMovement(){
        enemyDroidAI.followEnabled = true;
    }

    public void forceReset(){ //Slight animation bug is caused here
        enemyDroidAI.followEnabled = false;
        this.gameObject.transform.position = respawnPoint.transform.position;
        this.gameObject.GetComponent<Animator>().Play("respawn");
    }
}

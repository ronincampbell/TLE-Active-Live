using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPEnemies
{
    public class EnemyScript : MonoBehaviour
    {
        [Header("External Objects")]
        public GameObject enemyTarget;
        public GameObject GameManager;
        [HideInInspector] public bool enemyCanMove;
        private Vector3 enemyPos;
        private Animator enemyAnim;
        private bool enemyHasNotReachedTarget = true;
        private bool isFacingLeft = false;
        private bool deathAnimFinished = false;
        private bool enemyShouldDie = false;

        void Start()
        {
            enemyAnim = GetComponent<Animator>();
            enemyCanMove = true;
            enemyPos = this.transform.position; 
        }

        private void Update() 
        {
            if (enemyShouldDie && deathAnimFinished)
            {
                enemyAnim.SetTrigger("respawnDrone");
                enemyShouldDie = false;
                deathAnimFinished = false;
                this.transform.position = enemyPos;
                StartCoroutine(waitUntilCanMove());

            }
        }

        void FixedUpdate()
        {
            //Debug.Log("Enemy moving left: " + IsEnemyMovingLeft());
            CheckIfReachedTarget();
            bool isMovingLeft = IsEnemyMovingLeft();
            if (isFacingLeft != isMovingLeft) // only flip if direction has changed
            {
                FlipEnemySprite(isMovingLeft);
                isFacingLeft = isMovingLeft; // update direction
            }

            if (enemyCanMove && enemyHasNotReachedTarget)
            {
                enemyAnim.SetBool("reachedDestination", false);
                MoveTowardsTarget();
            }
        }

        public void ResetEnemyPosition()
        {
            enemyAnim.SetTrigger("killDrone");
            enemyShouldDie = true;
            enemyCanMove = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        private void MoveTowardsTarget()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, enemyTarget.transform.position, 0.1f);
        }

        private void CheckIfReachedTarget()
        {
            if (this.transform.position == enemyTarget.transform.position)
            {
                enemyHasNotReachedTarget = false;
                enemyAnim.SetBool("reachedDestination", true);
                //FlipEnemySprite(!IsEnemyMovingLeft());
            }
        }

        public void FlipEnemySprite(bool isMovingLeft)
        {
            Vector3 enemyScale = transform.localScale;
            enemyScale.x = isMovingLeft ? -Mathf.Abs(enemyScale.x) : Mathf.Abs(enemyScale.x);
            transform.localScale = enemyScale;
        }

        public bool IsEnemyMovingLeft()
        {
            return this.transform.position.x > enemyTarget.transform.position.x;
        }

        public void EnemyDeathAnimFinished()
        {
            deathAnimFinished = true;
        }

        private IEnumerator waitUntilCanMove(){
            yield return new WaitForSeconds(1.9f);
            //Debug.Log("Enemy can move again");
            enemyCanMove = true;
            enemyHasNotReachedTarget = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        public void ForceResetEnemy()
        {
            enemyAnim.SetTrigger("respawnDrone");
            enemyShouldDie = false;
            deathAnimFinished = false;
            enemyHasNotReachedTarget = true;
            this.transform.position = enemyPos;
        }

        // Statement that will check if the player is in the 2D Collider of the enemy
        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.gameObject.CompareTag("Player") && GameManager.GetComponent<GameManager>().playerIsDead == false)
            {
                //Debug.Log("Player has been hit by enemy");
                GameManager.GetComponent<GameManager>().killPlayer();
            }
        }
    }
}
    
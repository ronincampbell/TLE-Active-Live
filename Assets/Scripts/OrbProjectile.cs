using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    private bool canMove = true;

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
        
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Barrier"))
        {
            //Debug.Log("Barrier hit!");
            canMove = false;
            //this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponent<Animator>().SetTrigger("Detonate");
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.killPlayer();
            Destroy(gameObject);
            //Debug.Log("Player hit!");
        }
    }
    public void DestroyProjectileNow()
    {
        Destroy(gameObject);
    }
}

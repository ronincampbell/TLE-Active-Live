using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DoorScript : MonoBehaviour
{
    public bool isOpen = false;
    public SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closedSprite;

    private void Update() 
    {
        if (isOpen){
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.spriteRenderer.sprite = openSprite;
            this.GetComponent<Animator>().SetBool("isOpen", true);
        }else{
            this.gameObject.GetComponent<Collider2D>().enabled = true;
            this.spriteRenderer.sprite = closedSprite;
            this.GetComponent<Animator>().SetBool("isOpen", false);

        }    
    }
}

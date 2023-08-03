using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secret : MonoBehaviour
{
   public GameObject secretObject;
   
   private void OnTriggerEnter2D(Collider2D other) {
        secretObject.SetActive(true);
   }

   private void OnTriggerExit2D(Collider2D other) {
       secretObject.SetActive(false);
   }
}

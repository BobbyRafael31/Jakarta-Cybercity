using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitHealth : MonoBehaviour
{
   [SerializeField] private float healthValue;

   private void OnTriggerEnter2D(Collider2D collision) 
   {
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            collision.GetComponent<Health>().AddHealth(healthValue);
        }
   }
}

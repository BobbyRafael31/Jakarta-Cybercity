using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winning : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player")
        {
            FindObjectOfType<Win>().IsWon();
        }  
    }
}

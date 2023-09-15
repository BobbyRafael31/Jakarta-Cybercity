using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject bossObject;
    private enemyHealth bossEnemy;

    private void Start()
    {
        if (bossObject != null)
        {
            bossEnemy = bossObject.GetComponent<enemyHealth>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (bossEnemy == null || bossEnemy.currentHealth <= 0))
        {
            FindObjectOfType<Win>().IsWon();
        }
    }
}

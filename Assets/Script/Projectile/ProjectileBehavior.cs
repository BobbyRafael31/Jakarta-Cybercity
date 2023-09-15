using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float Speed = 4.5f;
    public float ProjectileDamage = 1f;

    private void Start()
    {
        // Ignore collisions between the projectile and objects with the "TagToIgnore" tag
        GameObject[] ignoreObjects = GameObject.FindGameObjectsWithTag("TagToIgnore");
        foreach (GameObject obj in ignoreObjects)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
        }
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * Speed;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            enemyHealth enemy = collider.gameObject.GetComponent<enemyHealth>();
            if (enemy != null)
            {
                enemy.TakeEnemyDamage(ProjectileDamage);
            }
        }

        if (!collider.CompareTag("Collectible") && !collider.CompareTag("Health"))
        {
            Destroy(gameObject);
        }
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

}

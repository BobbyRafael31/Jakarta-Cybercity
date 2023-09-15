using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    private bool isAnimationEventTriggered = false;

    // Call this method from the animation event
    public void TriggerDamageEvent()
    {
        isAnimationEventTriggered = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAnimationEventTriggered && collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            isAnimationEventTriggered = false;
        }
    }
}

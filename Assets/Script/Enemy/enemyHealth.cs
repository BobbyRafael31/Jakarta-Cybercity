using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth;
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    private MeleeEnemy meleeEnemy; // Reference to the MeleeEnemy script
    private BoxCollider2D boxCollider; // Reference to the BoxCollider2D component

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        meleeEnemy = GetComponent<MeleeEnemy>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void TakeEnemyDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            Debug.Log("Enemy hit");
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                dead = true;
                meleeEnemy.enabled = false; // Disable the MeleeEnemy script
                boxCollider.enabled = false; // Disable the BoxCollider2D component
                StartCoroutine(DestroyAfterAnimation());
                Debug.Log("Enemy dead");
            }
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // Wait for the death animation to finish

        // Disable other components if needed
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

        // Destroy the parent object
        Destroy(transform.parent.gameObject);
    }
    

}

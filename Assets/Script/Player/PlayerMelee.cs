using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
private enemyHealth enemyHealth;
[SerializeField] private LayerMask enemyLayer;

private int damage = 1;

   private void DamageEnemy()
   {
      enemyHealth.TakeEnemyDamage(damage);
   }
}

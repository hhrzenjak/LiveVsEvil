using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlow : Tower
{
    public override GameObject UpdateTarget()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position,
            transform.GetComponent<CircleCollider2D>().radius,
            LayerMask.GetMask("Enemy"));

        float shortestDistance = Mathf.Infinity;
        GameObject newTarget = null;

        foreach (Collider2D enemy in enemiesInRange)
        {
            // if its enemy from another player
            if (enemy.GetComponent<Enemy>().type != owner)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance && (!enemy.gameObject.GetComponent<Enemy>().isSlowed))
                {
                    shortestDistance = distanceToEnemy;
                    newTarget = enemy.gameObject;
                }
            }
        }

        return newTarget;
    }
    
}

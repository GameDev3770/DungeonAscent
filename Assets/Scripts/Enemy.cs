using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Health health;

    void Update()
    {
        if(health.currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void HitByRay()
    {
        health.TakeDamage(20);
    }


}

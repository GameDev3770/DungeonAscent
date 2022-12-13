using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Health health;


    void Update()
    {
        Debug.Log(health.currentHealth);
        if(health.currentHealth <= 0)
        {
            GameManager.Instance.player.GetComponent<Body>().changeActive(GetComponent<Body>().active);
            Debug.Log("destroy");
            Destroy(gameObject);
        }
    }

    void HitByRay()
    {
        health.TakeDamage(20);
    }


}

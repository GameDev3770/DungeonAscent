using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossKey : MonoBehaviour
{
    void HitByRay()
    {
        Debug.Log("Picked up Key");
        GameObject.Find("GameManager").GetComponent<GameManager>().bossKey = true;
        Destroy(gameObject);
    }
}

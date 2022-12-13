using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key1Key : MonoBehaviour
{
    void HitByRay()
    {
        Debug.Log("Picked up Key");
        GameObject.Find("GameManager").GetComponent<GameManager>().key1Key = true;
        Destroy(gameObject);
    }
}

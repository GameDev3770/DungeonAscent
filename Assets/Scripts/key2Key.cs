using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key2Key : MonoBehaviour
{
    void HitByRay()
    {
        Debug.Log("Picked up Key");
        GameObject.Find("GameManager").GetComponent<GameManager>().key2Key = true;
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prisonKey : MonoBehaviour
{
    void HitByRay()
    {
        Debug.Log("Picked up Key");
        GameObject.Find("GameManager").GetComponent<GameManager>().prisonKey = true;
        Destroy(gameObject);
    }
}

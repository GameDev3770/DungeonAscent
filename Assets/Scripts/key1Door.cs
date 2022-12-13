using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key1Door : MonoBehaviour
{
    bool clicked1 = false;
    bool opened1 = false;

    void Update()
    {
        if(GameObject.Find("GameManager").GetComponent<GameManager>().key1Key == true && clicked1 == true && opened1 == false)
        {
            Debug.Log("You Managed to fopen the door!");
            transform.Rotate(0, 90, 0);
            opened1 = true;
        }
    }

    void HitByRay() //runs when the door is clicked on
    {
        clicked1 = true;
    }
}

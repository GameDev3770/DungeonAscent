using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key2Door : MonoBehaviour
{
    bool clicked2 = false;
    bool opened2 = false;

    void Update()
    {
        if(GameObject.Find("GameManager").GetComponent<GameManager>().key2Key == true && clicked2 == true && opened2 == false)
        {
            Debug.Log("You Managed to fopen the door!");
            transform.Rotate(0, 90, 0);
            opened2 = true;
        }
    }

    void HitByRay() //runs when the door is clicked on
    {
        clicked2 = true;
    }
}

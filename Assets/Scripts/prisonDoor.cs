using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prisonDoor : MonoBehaviour
{
    bool clicked = false;
    bool opened = false;

    void Update()
    {
        if(GameObject.Find("GameManager").GetComponent<GameManager>().prisonKey == true && clicked == true && opened == false)
        {
            Debug.Log("You Managed to escape!");
            transform.Rotate(0, 90, 0);
            opened = true;
        }
    }

    void HitByRay() //runs when the door is clicked on
    {
        clicked = true;
    }
}

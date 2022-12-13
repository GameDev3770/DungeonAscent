using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossDoor : MonoBehaviour
{
    bool clickedB = false;
    bool openedB = false;

    void Update()
    {
        if(GameObject.Find("GameManager").GetComponent<GameManager>().bossKey == true && clickedB == true && openedB == false)
        {
            Debug.Log("You Managed to find me!");
            transform.Rotate(0, 90, 0);
            openedB = true;
        }
    }

    void HitByRay() //runs when the door is clicked on
    {
        clickedB = true;
    }
}

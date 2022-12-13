using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // add booleans for objects that the player can pickup.

    public static GameManager Instance;
    public bool prisonKey; // checks if starting area prison key was picked up.
    public GameObject player;
    private void Awake(){
        Instance = this;
    }

}

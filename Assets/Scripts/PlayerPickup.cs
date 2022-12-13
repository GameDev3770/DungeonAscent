using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    //also using this for attacking

    public GameObject player;
    static public float maxDistance;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition); //declaring ray

            if(Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                maxDistance = Vector3.Distance(player.transform.position, hitInfo.transform.position); //calculating distance from player to object clicked

                if(maxDistance < 1.5f) //player can only interact with object 1.5m away
                {
                    hitInfo.transform.SendMessage("HitByRay", null, SendMessageOptions.DontRequireReceiver); // sending info to the object hit telling it to run the "HitByRay" function
                }
            }
        }
    }
}

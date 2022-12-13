using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject weaponToPickUp;
    public Transform weaponHolder;

    void HitByRay()
    {
        gameObject.SetActive(false);    //disables gameobject in world
        weaponToPickUp.transform.parent = weaponHolder.transform;
    }
}

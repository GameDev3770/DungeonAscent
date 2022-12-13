using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    
    [Header("Weapon Stats")]
    public int weaponDamage;
    public float range;
    public int accuracy;
    public int critRate;

    [Header("References")]
    public GameObject player;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray myRay = new Ray(player.transform.position + new Vector3(0,0.25f,0), player.transform.forward);
            RaycastHit hit;

            if(Physics.Raycast(myRay, out hit, range))
            {
                if(hit.collider.tag == "Enemy")
                {
                    hit.transform.SendMessage("DamageEnemy", weaponDamage, SendMessageOptions.DontRequireReceiver); // sending info to the object hit telling it to run the "HitByRay" function
                }
            }
        }
    }
}

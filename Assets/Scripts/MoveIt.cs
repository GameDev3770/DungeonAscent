using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIt : MonoBehaviour
{
    public float speed = 1;
    public Vector3 target;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate (target * speed * Time.deltaTime);     

            if(gameObject.CompareTag("Lava2")){
        //Debug.Log("lava2");
        if(transform.position.z >= 23){
           target = Vector3.back;
      } 
    else if(transform.position.z <= 15.33){
           target = Vector3.forward;
      }
    }


    if(transform.position.x >=9){
           target = Vector3.left;
      } 
    else if(transform.position.x <= 3){
           target = Vector3.right;
      }
       // transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

    }


}
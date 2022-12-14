using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public int active;
    public Health health;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(active).gameObject.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void disableActive(){

    }
    public void enableActive(){
    }
    public void changeActive(int newActive){
        transform.GetChild(active).gameObject.SetActive(false);
        transform.GetChild(newActive).gameObject.SetActive(true);
        active = newActive;
        health.TakeDamage(-25);

    }
}

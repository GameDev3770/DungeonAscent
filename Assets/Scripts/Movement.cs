using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    Vector3 up = Vector3.zero,
    right = new Vector3(0,90,0),
    down = new Vector3(0,180,0),
    left = new Vector3(0,270,0),
    currentDir = Vector3.zero;

    Vector3 nextPos, destination, direction;

    float speed = 5f;
    float rayLen = 1f;
    bool canMove;

    Animator animator;
    public Health health;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentDir = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    void OnTriggerStay(Collider c){

        if(c.CompareTag("Lava3") || c.CompareTag("Lava2"))
        {
            Debug.Log("on the lava");     //now you can do ur health-- here, and as long as the player stays inside the lava place, it will repeat.      
        }
    }


    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        if(Input.GetMouseButtonDown(1)){
            animator.SetTrigger("Attacking");
        }

        if(Input.GetKeyDown(KeyCode.W) && !OnLava(Vector3.forward))
        {
            nextPos = Vector3.forward;
            currentDir = up;
            canMove = true;
        }
        if(Input.GetKeyDown(KeyCode.A) && !OnLava(Vector3.left))
        {
            nextPos = Vector3.left;
            currentDir = left;
            canMove = true;
        }
        if(Input.GetKeyDown(KeyCode.S) && !OnLava(Vector3.back))
        {
            nextPos = Vector3.back;
            currentDir = down;
            canMove = true;
        }
        if(Input.GetKeyDown(KeyCode.D) && !OnLava(Vector3.right))
        {
            nextPos = Vector3.right;
            currentDir = right;
            canMove = true;
        }

        if(Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = currentDir;

            if(canMove)
            {
                if(Valid())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                }
            }
        }
        animator.SetBool("Moving",transform.position!=destination);
    }

    // check if player collided with an obstacle (wall, table, etc...) uses raycast
    bool Valid()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0,0.25f,0), transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(myRay, out hit, rayLen))
        {
            if(hit.collider.tag == "obstacle" || hit.collider.tag == "Enemy")
            {
                return false;
            }
        }
        return true;
    }

<<<<<<< HEAD
    bool OnLava(Vector3 offset)
=======
     bool OnLava()
>>>>>>> e9434297f5fa9a3ed1f8627688c1a862b7ada51c
    {
        Ray myRay = new Ray(transform.position + offset + new Vector3(0f, 1f, 0f),  -transform.up);
        Debug.DrawRay(transform.position + offset + new Vector3(0f, 0.5f, 0f),  -transform.up, Color.green, 3);
        RaycastHit hit;

        if(Physics.Raycast(myRay, out hit, rayLen))
        {
            Debug.Log(false);
            return false;
        }
        Debug.Log(true);
        return true;
    }
    

}

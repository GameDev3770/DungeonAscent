using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEnemy : MonoBehaviour
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

    void Start()
    {
        animator = GetComponent<Animator>();
        currentDir = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if(Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = currentDir;

            if(canMove)
            {
                destination = transform.position + nextPos;
                direction = nextPos;
                canMove = false;
            }
        }
        animator.SetBool("Moving",transform.position!=destination);
    }

    public void UpdateMove(float[] newPos) {
        float dif_x = newPos[0] - transform.position.x;
        float dif_y = newPos[1] - transform.position.z;
        // Vector3 newCoords = new Vector3(newPos[0], transform.position.y, newPos[1]);

        if (dif_x > 0) {
            nextPos = Vector3.right;
            currentDir = left;
            canMove = true;
        }
        else if (dif_x < 0) {
            nextPos = Vector3.left;
            currentDir = left;
            canMove = true;
        }
        else if (dif_y > 0) {
            nextPos = Vector3.forward;
            currentDir = up;
            canMove = true;
        } 
        else if (dif_y < 0) {
            nextPos = Vector3.back;
            currentDir = down;
            canMove = true;
        }
        // Move();
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
}

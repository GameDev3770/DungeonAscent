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

    void Start()
    {
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

        if(Input.GetKeyDown(KeyCode.W))
        {
            nextPos = Vector3.forward;
            currentDir = up;
            canMove = true;
            OnLava();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            nextPos = Vector3.left;
            currentDir = left;
            canMove = true;
            OnLava();

        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            nextPos = Vector3.back;
            currentDir = down;
            canMove = true;
            OnLava();

        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            nextPos = Vector3.right;
            currentDir = right;
            canMove = true;
            OnLava();

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

    bool OnLava()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0f,0.25f,1.5f), -transform.up);
        RaycastHit hit;

        if(Physics.Raycast(myRay, out hit, rayLen))
        {
            if(hit.collider.tag == "Lava")
            {
                Debug.Log(true);
                return true;
            }
        }
        return false;
    }
}

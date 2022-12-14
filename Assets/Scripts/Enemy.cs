using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Health health;

    GameObject Environment;
    GameObject Player;
    Animator animator;

    float maxticker = 100;
    public float tick = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        this.Environment = GameObject.Find("Environment");
        this.Player = GameObject.Find("Player");
    }

    void Update()
    {
        Debug.Log(health.currentHealth);
        if(health.currentHealth <= 0)
        {
            GameManager.Instance.player.GetComponent<Body>().changeActive(GetComponent<Body>().active);
            Debug.Log("destroy");
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        if (tick >= maxticker) {
            //Vector3 vec = transform.position;
            float[] temp = MakeMove();
            GetComponent<MovementEnemy>().UpdateMove(temp);
            //vec.x = temp[0];
            //vec.z = temp[1];
            //transform.position = vec;
            tick = 0;
            //animation
        }
        tick++;
    }

    void HitByRay()
    {
        health.TakeDamage(20);
    }

    float[] MakeMove()
    {
        List<Node> output = Environment.GetComponent<PathFinderAPI>().GetPathing(this.gameObject, this.Player);
        if (1 < output.Count && output.Count <= 6 && IsEmpty(new Vector3(output[0].x, 5, output[0].y)))
        {
            return new float[] { output[0].x, output[0].y };
        } 
        else if(output.Count == 1)
        {
            Debug.Log("Front");
            //attack
        }
        return new float[] { transform.position.x, transform.position.z };
    }

    public bool IsEmpty(Vector3 worldPosition)
    {
        RaycastHit hit;
        bool a = Physics.Raycast(worldPosition, Vector3.down, out hit, 10, 1 << LayerMask.NameToLayer("Entity"));
        return !a;
    }
}

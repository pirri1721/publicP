using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomAgentMovement : MonoBehaviour
{
    public Transform[] points;
    private NavMeshAgent agent;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        anim.SetTrigger("ia");
        agent.destination = points[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", agent.speed);

        if(agent.remainingDistance < 0.5f)
        {

        }
    }
}

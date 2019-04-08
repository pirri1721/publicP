using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomAgentMovement : MonoBehaviour
{
    public Transform[] points;
    private NavMeshAgent agent;
    private Animator anim;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        anim.SetTrigger("ia");
        index = 0;
        agent.destination = points[index].position;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", agent.speed);

        if(agent.remainingDistance < 1.5f)
        {
            Debug.Log("lol");
            Debug.Log(agent.remainingDistance);
            Debug.Log(agent.destination);
            agent.destination = points[NextIndex()].position;
        }
    }

    public int NextIndex()
    {
        if(index + 1 >= points.Length)
        {
            return 0;
        }
        else
        {
            return index++;
        }
    }
}

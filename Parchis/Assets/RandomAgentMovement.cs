using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomAgentMovement : MonoBehaviour
{
    public Transform[] points;
    private NavMeshAgent agent;
    private Animator anim;
    public int index;
    private bool timeElpsed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        anim.SetTrigger("ia");
        index = 0;
        agent.SetDestination(points[index].position);
        timeElpsed = true;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", agent.speed);

        if(agent.remainingDistance < 1.5f && timeElpsed)
        {
            timeElpsed = false;
            Debug.Log("lol");
            Debug.Log(agent.remainingDistance);
            Debug.Log(agent.destination);
            StartCoroutine(IA());
        }
    }

    private IEnumerator IA()
    {
        agent.Stop();
        agent.ResetPath();
        yield return new WaitForSeconds(1.5f);
        int i = NextIndex();
        index = i;
        agent.SetDestination(points[index].position);
    }

    public int NextIndex()
    {
        if(index + 1 >= points.Length)
        {
            return 0;
        }
        else
        {
            timeElpsed = true;
            index++;
            return index;
        }
    }
}

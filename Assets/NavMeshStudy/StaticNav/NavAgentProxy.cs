using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentProxy : MonoBehaviour
{
    public Transform target = null;

    public NavMeshAgent navAgent = null;
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if (navAgent && target)
            navAgent.SetDestination(target.position);
    }
}

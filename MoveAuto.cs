using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAuto : MonoBehaviour {

    public Transform Passing1;
    public Transform Passing2;
    public Transform Goal;
    public List<Transform> Passes;
    private bool Pass1_flug = false;
    private bool Pass2_flug = false;
    private bool Goal_flug = false;
    private bool Next = false;

    void Start()
    {

    }

    void Update()
    {

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.stoppingDistance = 1.0f;
        
        agent.destination = Passing1.position;
        if (Vector3.Distance(Passing1.position, transform.position) < 1.5f && !Pass1_flug)
        {
            Pass1_flug = true;
            agent.destination = Passing2.position;
        }else if(Vector3.Distance(Passing2.position, transform.position) < 1.5f || Pass2_flug)
        {
            Pass2_flug = true;
            agent.destination = Goal.position;
        }else if (Goal_flug)
        {

        }
        
        //agent.destination = Goal.position;
    }

}

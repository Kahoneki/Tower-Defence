using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public Transform end;

    void Start() {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = end.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteveAI : MonoBehaviour
{
    public Transform goal;
    public Transform[] patrolPoints;
    public float targetDistance;
    public float moveSpeed;

    private int currentWaypoint = 0;

    private void Move()
    {
        Vector3 targetDir = goal.position - transform.position;
        if(Vector3.Distance(transform.position, goal.position) > targetDistance)
            transform.Translate(targetDir.normalized * moveSpeed);
    }

    void Patrol()
    {
        goal = patrolPoints[currentWaypoint];
        Vector3 targetDir = goal.position - transform.position;
        if (Vector3.Distance(transform.position, goal.position) > targetDistance)
            transform.Translate(targetDir.normalized * moveSpeed);
        else
        {
            if (currentWaypoint < patrolPoints.Length)
            {
                currentWaypoint++;
            }
        }
        if (currentWaypoint >= patrolPoints.Length)
            currentWaypoint = 0;
    }

    private void LateUpdate()
    {
        Patrol();
    }
}

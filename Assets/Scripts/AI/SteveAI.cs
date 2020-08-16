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
        if(Vector3.Distance(transform.position, goal.position) > targetDistance)
        {
            transform.Translate(transform.forward * moveSpeed, Space.World);
        }
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

    IEnumerator Wander()
    {
        while(true)
        {
            bool reachedGoal = false;
            Vector2 randomGoal = Random.insideUnitCircle * 5;
            goal.position = new Vector3(randomGoal.x, goal.position.y, randomGoal.y);
            Vector3 targetDir = goal.position - transform.position;
            while(!reachedGoal)
            {
                transform.Translate(targetDir.normalized * moveSpeed);
                if (Vector3.Distance(transform.position, goal.position) <= targetDistance)
                    reachedGoal = true;
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }
    }

    private void Start()
    {
        //StartCoroutine(Wander());
    }

    void CalculateAngle()
    {
        Vector3 fwd = this.transform.forward;
        Vector3 targetDir = goal.position - transform.position;

        float dot = fwd.x * targetDir.x + fwd.z * targetDir.z;
        float angle = Mathf.Acos(dot / (fwd.magnitude * targetDir.magnitude));

        Debug.DrawRay(transform.position, fwd * 5f, Color.green);
        Debug.DrawRay(transform.position, targetDir * 5f, Color.red);
        Debug.Log(angle * Mathf.Rad2Deg);
        Debug.Log(Vector3.Angle(transform.position, goal.position));

        int clockWise = 1;
        if(Cross(fwd, targetDir).y < 0)
        {
            clockWise = -1;
        }

        transform.Rotate(0, (angle * Mathf.Rad2Deg * clockWise) * 0.25f, 0);
    }

    Vector3 Cross(Vector3 v, Vector3 w)
    {
        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.z * w.x - v.x * w.z;
        float zMult = v.x * w.y - v.y * w.x;

        return new Vector3(xMult, yMult, zMult);
    }

    void TravelToGoal()
    {
        CalculateAngle();
        Move();
    }

    public void UpdateAI()
    {
        TravelToGoal();
        //Patrol();
    }
}

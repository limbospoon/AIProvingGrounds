using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingAI : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public float targetDistance;

    public Transform[] checkPoints;
    private Transform currentCheckPoint;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentCheckPoint = checkPoints[currentIndex];
    }

    void LookAtCurrentCheckPoint()
    {
        Vector3 targetDir = (currentCheckPoint.position - transform.position).normalized;
        Quaternion lookTarget = Quaternion.LookRotation(new Vector3(targetDir.x, 0f, targetDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookTarget, Time.deltaTime * turnSpeed);
    }

    void Drive()
    {
        LookAtCurrentCheckPoint();

        transform.Translate(0, 0, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentCheckPoint.position) <= targetDistance)
        {
            if (currentIndex < checkPoints.Length)
                currentIndex++;
            else if(currentIndex >= checkPoints.Length)
                currentIndex = 0;
            currentCheckPoint = checkPoints[currentIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Drive();
    }
}

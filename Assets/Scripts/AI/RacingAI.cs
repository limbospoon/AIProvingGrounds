using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingAI : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public float targetDistance;
    public float lookAhead = 10.0f;

    public Transform[] checkPoints;
    private Transform currentCheckPoint;
    private int currentIndex = 0;
    GameObject tracker;

    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        currentCheckPoint = checkPoints[currentIndex];
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
    }

    void ProgressTracker()
    {
        if(Vector3.Distance(tracker.transform.position, transform.position) < lookAhead)
        {
            tracker.transform.LookAt(currentCheckPoint.transform);
            tracker.transform.Translate(0, 0, (moveSpeed + 2.5f) * Time.deltaTime);
        }

        if (Vector3.Distance(tracker.transform.position, currentCheckPoint.position) <= targetDistance)
        {
            int max = checkPoints.Length;
            if (currentIndex >= max)
                currentIndex = 0;

            currentCheckPoint = checkPoints[currentIndex++];
        }
    }

    void LookAtCurrentCheckPoint()
    {
        Vector3 targetDir = (tracker.transform.position - transform.position).normalized;
        Quaternion lookTarget = Quaternion.LookRotation(new Vector3(targetDir.x, 0f, targetDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookTarget, Time.deltaTime * turnSpeed);
    }

    void Drive()
    {
        LookAtCurrentCheckPoint();

        transform.Translate(0, 0, moveSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        ProgressTracker();
        Drive();
    }
}

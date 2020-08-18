using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteveAI : MonoBehaviour
{
    public Transform goal;
    public Transform[] patrolPoints;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public GameObject turret;
    public GameObject target;
    public float targetDistance;
    public float moveSpeed;
    public float projectileSpeed;
    public bool canShoot = true;

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

    void ShootTarget()
    {
        Vector3 targetDir = (target.transform.position - transform.position).normalized;
        Quaternion lookRoation = Quaternion.LookRotation(new Vector3(targetDir.x, 0, targetDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRoation, Time.deltaTime * 5.0f);

        float? angle = RoateTurret();

        if(angle != null && Vector3.Angle(targetDir, transform.forward) < 10.0f)
        {
            FireBullet();
        }
        Debug.Log(angle);
    }

    void CanShootAgain()
    {
        canShoot = true;
    }

    void FireBullet()
    {
        if(canShoot)
        {
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            bullet.GetComponent<Rigidbody>().velocity = projectileSpeed * turret.transform.forward;
            canShoot = false;
            Invoke("CanShootAgain", 0.5f);
        }
    }

    float? RoateTurret()
    {
        float? angle = CalculateTargetAngle(true);

        if(angle != null)
        {
            turret.transform.localEulerAngles = new Vector3(360.0f - (float)angle, 0f, 0f);
        }
        return angle;
    }

    float? CalculateTargetAngle(bool low)
    {
        Vector3 targetDir = target.transform.position - turret.transform.position;
        float y = targetDir.y;
        targetDir.y = 0f;
        float x = targetDir.magnitude;
        float gravity = 9.81f;
        float speedSqr = moveSpeed * moveSpeed;
        float underTheSqr = (speedSqr * speedSqr) - gravity * (gravity * x * x + 2 * y * speedSqr);

        if (underTheSqr >= 0f)
        {
            float root = Mathf.Sqrt(underTheSqr);
            float highAngle = speedSqr + root;
            float lowAngle = speedSqr - root;

            if (low)
            {
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            }
            else
            {
                return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);
            }
        }
        else
            return null;
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
        //TravelToGoal();
        //Patrol();
        ShootTarget();
    }
}

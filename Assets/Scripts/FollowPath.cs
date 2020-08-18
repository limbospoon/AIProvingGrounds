using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPath : MonoBehaviour
{
    public GameObject wpManager;
    GameObject[] wps;
    NavMeshAgent agent;


    private void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        agent = GetComponent<NavMeshAgent>();
    }

    public void GoToHeli()
    {
        agent.SetDestination(wps[4].transform.position);
    }

    public void GoToRuin()
    {
        agent.SetDestination(wps[7].transform.position);
    }

    public void GoToOilRigs()
    {
        agent.SetDestination(wps[6].transform.position);
    }

    private void LateUpdate()
    {

    }
}

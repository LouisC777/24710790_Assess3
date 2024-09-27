using UnityEngine;
using System.Collections;

public class PacStudentMovement : MonoBehaviour
{
    public Transform[] waypoints;        
    public float speed = 5f;            
    public float tweenDuration = 1f;    
    private int currentWaypointIndex = 0; 

    void Update()
    {
     
        if (currentWaypointIndex < waypoints.Length)
        {
            
            MoveTowardsWaypoint();
        }
    }

    
    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

     
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++; 
        }
    }
}

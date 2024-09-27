using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public Transform[] waypoints;         
    public float speed = 5f;              
    public AudioClip moveSound;          
    private AudioSource audioSource;      
    private Animator animator;             
    private int currentWaypointIndex = 0; 

    void Start()
    {
        animator = GetComponent<Animator>(); 
        audioSource = GetComponent<AudioSource>();      
        audioSource.loop = true;  
        audioSource.clip = moveSound;  
    }

    void Update()
    {
        
        if (waypoints.Length > 0)
        {
            MoveTowardsWaypoint();
        }
        else
        {
            
            audioSource.Stop();
        }
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        
        UpdateAnimation(direction);

        
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++; 

            
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; 
            }
        }

        
        if (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); 
            }
        }
        else
        {
            audioSource.Stop(); 
        }
    }

    void UpdateAnimation(Vector3 direction)
    {
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) 
        {
            if (direction.x < 0) 
            {
                animator.SetTrigger("MoveLeft");
            }
            else if (direction.x > 0) 
            {
                animator.SetTrigger("MoveRight");
            }
        }
        else 
        {
            if (direction.y > 0) 
            {
                animator.SetTrigger("MoveUp");
            }
            else if (direction.y < 0) 
            {
                animator.SetTrigger("MoveDown");
            }
        }
    }
}

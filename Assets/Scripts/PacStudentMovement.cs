using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public Transform[] waypoints;         
    public float speed = 5f;             
    public AudioClip moveSound;          
    private AudioSource audioSource;    
    public Sprite upSprite;              
    public Sprite downSprite;            
    public Sprite leftSprite;           
    public Sprite rightSprite;           

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private int currentWaypointIndex = 0; // Index of the current waypoint

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
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

       
        UpdateSprite(direction);

       
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

    void UpdateSprite(Vector3 direction)
    {
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0) 
            {
                spriteRenderer.sprite = leftSprite;
            }
            else if (direction.x > 0) 
            {
                spriteRenderer.sprite = rightSprite;
            }
        }
        else 
        {
            if (direction.y > 0) 
            {
                spriteRenderer.sprite = upSprite;
            }
            else if (direction.y < 0)
            {
                spriteRenderer.sprite = downSprite;
            }
        }
    }
}

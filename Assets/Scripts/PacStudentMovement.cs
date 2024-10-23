using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    // public Transform[] waypoints;         // Removed: No longer using waypoints
    public float speed = 5f;              // Retained: May use for manual movement
    public AudioClip moveSound;          // Retained: For movement sound effects
    private AudioSource audioSource;      // Retained: To manage audio playback
    private Animator animator;             // Retained: For character animations
    // private int currentWaypointIndex = 0; // Removed: No longer tracking waypoints

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = moveSound;
    }

    void Update()
    {
        // Removed: Waypoint movement logic
        // You can add manual movement controls here if needed
    }

    // Removed: MoveTowardsWaypoint method

    void UpdateAnimation(Vector3 direction)
    {
        // Retained: To handle animations based on movement direction
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

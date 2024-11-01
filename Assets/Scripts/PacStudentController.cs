using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private KeyCode lastInput, currentInput;
    private Vector3 targetPosition, currentPosition;
    private bool isMoving = false;

    // Animation and audio components
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip pelletClip;
    public AudioClip moveClip;

    // Tilemap references
    public Tilemap wallTilemap;

    // Dust particle effect
    public ParticleSystem dustParticlePrefab; // Assign the dust particle prefab here
    private ParticleSystem dustParticleInstance;

    void Start()
    {
        currentPosition = transform.position;
        targetPosition = currentPosition;

        // Instantiate the dust particles and parent it to PacStudent
        dustParticleInstance = Instantiate(dustParticlePrefab, transform.position, Quaternion.identity);
        dustParticleInstance.transform.SetParent(transform);
        dustParticleInstance.Stop(); // Ensure it starts in a stopped state
    }

    void Update()
    {
        GatherInput();

        if (!isMoving)
        {
            if (TryMove(lastInput)) return; // Try to move with last input
            if (TryMove(currentInput)) return; // Try to move with current input
        }

        // Control particle effect based on movement state
        if (isMoving && !dustParticleInstance.isPlaying)
        {
            dustParticleInstance.Play(); // Start particle effect when moving
        }
        else if (!isMoving && dustParticleInstance.isPlaying)
        {
            dustParticleInstance.Stop(); // Stop particle effect when idle
        }
    }

    void GatherInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) lastInput = KeyCode.W;
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = KeyCode.S;
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = KeyCode.D;
    }

    bool TryMove(KeyCode direction)
    {
        Vector3 moveDirection = GetDirectionFromKey(direction);
        Vector3 newPosition = currentPosition + moveDirection;

        if (IsWalkable(newPosition))
        {
            currentInput = direction;
            targetPosition = newPosition;
            SetMovementAnimation(direction); // Set animation only if moving
            StartCoroutine(MoveToPosition(targetPosition));
            return true;
        }
        return false;
    }

    Vector3 GetDirectionFromKey(KeyCode key)
    {
        if (key == KeyCode.W) return Vector3.up;
        if (key == KeyCode.A) return Vector3.left;
        if (key == KeyCode.S) return Vector3.down;
        if (key == KeyCode.D) return Vector3.right;
        return Vector3.zero;
    }

    bool IsWalkable(Vector3 position)
    {
        TileBase tile = wallTilemap.GetTile(wallTilemap.WorldToCell(position));
        return tile == null; // If there's no tile, it's walkable
    }

    void SetMovementAnimation(KeyCode direction)
    {
        animator.SetBool("isMoving", true);
        animator.ResetTrigger("moveUp");
        animator.ResetTrigger("moveDown");
        animator.ResetTrigger("moveLeft");
        animator.ResetTrigger("moveRight");

        // Trigger the correct animation based on the direction
        if (direction == KeyCode.W) animator.SetTrigger("moveUp");
        else if (direction == KeyCode.A) animator.SetTrigger("moveLeft");
        else if (direction == KeyCode.S) animator.SetTrigger("moveDown");
        else if (direction == KeyCode.D) animator.SetTrigger("moveRight");
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;
        animator.SetBool("isMoving", true);

        audioSource.Play();

        float elapsedTime = 0;
        float journeyLength = Vector3.Distance(currentPosition, targetPosition);

        while (elapsedTime < journeyLength / moveSpeed)
        {
            transform.position = Vector3.Lerp(currentPosition, target, (elapsedTime * moveSpeed) / journeyLength);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        currentPosition = target;
        isMoving = false;

        // Stop animation and audio when movement finishes
        animator.SetBool("isMoving", false);
        audioSource.Stop();
    }

    bool IsPellet(Vector3 position)
    {
        // Implement logic to determine if the target position has a pellet
        return false; // Example placeholder
    }
}

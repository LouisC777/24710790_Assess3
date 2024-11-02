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
    public AudioClip pelletClip; // Sound effect for eating a pellet
    public AudioClip moveClip;
    public AudioClip wallCollisionSound; // Wall collision sound effect

    // Tilemap references
    public Tilemap wallTilemap;
    public Tilemap pelletTilemap; // Reference to the pellet tilemap

    // Dust particle effect
    public ParticleSystem dustParticlePrefab; // Assign the dust particle prefab here
    private ParticleSystem dustParticleInstance;

    // Wall collision particle effect
    public ParticleSystem wallCollisionParticlePrefab; // Assign the wall collision particle prefab here
    private bool hasCollidedWithWall = false; // Flag to track wall collisions

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
            hasCollidedWithWall = false; // Reset the collision flag when moving

            // Check if there's a pellet at the new position
            if (IsPellet(newPosition))
            {
                DestroyPellet(newPosition); // Destroy the pellet if it exists
            }

            return true;
        }
        else
        {
            // If not walkable, handle wall collision
            HandleWallCollision();
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

    void HandleWallCollision()
    {
        // Play the wall collision sound effect
        audioSource.PlayOneShot(wallCollisionSound);

        // Play wall collision particle effect only if not already collided
        if (!hasCollidedWithWall)
        {
            TriggerWallCollisionEffect(transform.position);
            hasCollidedWithWall = true; // Set the flag to true to prevent further particle effects
        }
    }

    void TriggerWallCollisionEffect(Vector3 position)
    {
        ParticleSystem wallEffect = Instantiate(wallCollisionParticlePrefab, position, Quaternion.identity);
        wallEffect.Play();
        Destroy(wallEffect.gameObject, 1f); // Destroy the particle effect after 1 second
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
        // Check if there is a pellet at the current position
        Vector3Int tilePosition = pelletTilemap.WorldToCell(position);
        TileBase pelletTile = pelletTilemap.GetTile(tilePosition);
        return pelletTile != null; // Return true if there is a pellet
    }

    void DestroyPellet(Vector3 position)
    {
        // Play the pellet eating sound effect
        audioSource.PlayOneShot(pelletClip);

        // Get the tile position and destroy the pellet tile
        Vector3Int tilePosition = pelletTilemap.WorldToCell(position);
        pelletTilemap.SetTile(tilePosition, null); // Set the tile to null to destroy it
        // Here you can add logic to update the player's score
    }
}

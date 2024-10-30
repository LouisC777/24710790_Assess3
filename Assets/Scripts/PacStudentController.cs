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

    void Start()
    {
        currentPosition = transform.position;
        targetPosition = currentPosition;
    }

    void Update()
    {
        GatherInput();

        if (!isMoving)
        {
            TryMove(lastInput);

            if (!isMoving && currentInput != KeyCode.None)
            {
                TryMove(currentInput);
            }
        }
    }

    void GatherInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            lastInput = KeyCode.W;
            animator.SetTrigger("MoveUp"); // Trigger up movement animation
        }
        else if (Input.GetKeyDown(KeyCode.A)) 
        {
            lastInput = KeyCode.A;
            animator.SetTrigger("MoveLeft"); // Trigger left movement animation
        }
        else if (Input.GetKeyDown(KeyCode.S)) 
        {
            lastInput = KeyCode.S;
            animator.SetTrigger("MoveDown"); // Trigger down movement animation
        }
        else if (Input.GetKeyDown(KeyCode.D)) 
        {
            lastInput = KeyCode.D;
            animator.SetTrigger("MoveRight"); // Trigger right movement animation
        }
    }

    void TryMove(KeyCode direction)
    {
        Vector3 moveDirection = GetDirectionFromKey(direction);
        Vector3 newPosition = currentPosition + moveDirection;

        if (IsWalkable(newPosition))
        {
            currentInput = direction;
            targetPosition = newPosition;
            StartCoroutine(MoveToPosition(targetPosition));
        }
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

    System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;
        animator.SetBool("isMoving", true); // Start movement animation

        // Check if PacStudent is about to eat a pellet
        if (IsPellet(target))
        {
            audioSource.clip = pelletClip;
        }
        else
        {
            audioSource.clip = moveClip;
        }
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

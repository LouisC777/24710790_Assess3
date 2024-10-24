using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float speed = 5f; // Speed at which PacStudent moves
    private Vector3 direction; // Current movement direction
    private Vector3Int currentGridPosition; // Current grid position of PacStudent
    private LevelMap levelMap; // Reference to the LevelMap component

    void Start()
    {
        direction = Vector3.zero; // Initialize direction to zero
        currentGridPosition = Vector3Int.RoundToInt(transform.position); // Get the initial grid position
        levelMap = FindObjectOfType<LevelMap>(); // Get the LevelMap component
    }

    void Update()
    {
        HandleInput();

        // Move continuously in the current direction
        if (direction != Vector3.zero)
        {
            Move();
        }
    }

    void HandleInput()
    {
        // If PacStudent is not currently moving, check for input
        if (Input.GetKeyDown(KeyCode.W) && IsWalkable(currentGridPosition + Vector3Int.up))
        {
            direction = Vector3.up; // Move up
        }
        else if (Input.GetKeyDown(KeyCode.A) && IsWalkable(currentGridPosition + Vector3Int.left))
        {
            direction = Vector3.left; // Move left
        }
        else if (Input.GetKeyDown(KeyCode.S) && IsWalkable(currentGridPosition + Vector3Int.down))
        {
            direction = Vector3.down; // Move down
        }
        else if (Input.GetKeyDown(KeyCode.D) && IsWalkable(currentGridPosition + Vector3Int.right))
        {
            direction = Vector3.right; // Move right
        }
    }

    void Move()
    {
        // Move towards the current direction
        Vector3 targetPosition = transform.position + direction * speed * Time.deltaTime;

        // Check if the next position is walkable
        if (IsWalkable(Vector3Int.RoundToInt(targetPosition)))
        {
            transform.position = targetPosition; // Update the position
            currentGridPosition = Vector3Int.RoundToInt(transform.position); // Update grid position
        }
        else
        {
            // If not walkable, stop moving in that direction
            direction = Vector3.zero; // Stop moving
        }
    }

    bool IsWalkable(Vector3Int gridPosition)
    {
        return levelMap.IsWalkable(gridPosition); // Use the LevelMap's IsWalkable method
    }
}

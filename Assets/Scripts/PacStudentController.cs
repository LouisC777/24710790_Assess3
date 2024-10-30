using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private KeyCode lastInput, currentInput;
    private Vector3 targetPosition, currentPosition;
    private bool isMoving = false;

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
        if (Input.GetKeyDown(KeyCode.W)) lastInput = KeyCode.W;
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = KeyCode.S;
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = KeyCode.D;
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
        // Check with LevelGenerator.cs or other logic if this position is walkable
        return true; // Example, replace with actual check
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;
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
    }
}

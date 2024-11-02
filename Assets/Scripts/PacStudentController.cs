using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private KeyCode lastInput, currentInput;
    private Vector3 targetPosition, currentPosition;
    private bool isMoving = false;


    public Animator animator;
    public AudioSource audioSource;
    public AudioClip pelletClip;
    public AudioClip moveClip;
    public AudioClip wallCollisionSound; 

   
    public Tilemap wallTilemap;
    public Tilemap pelletTilemap; 

 
    public ParticleSystem dustParticlePrefab;
    private ParticleSystem dustParticleInstance;


    public ParticleSystem wallCollisionParticlePrefab;
    private bool hasCollidedWithWall = false;

    private ScoreManager scoreManager; 

    void Start()
    {
        currentPosition = transform.position;
        targetPosition = currentPosition;

       
        scoreManager = Object.FindFirstObjectByType<ScoreManager>();

        
        dustParticleInstance = Instantiate(dustParticlePrefab, transform.position, Quaternion.identity);
        dustParticleInstance.transform.SetParent(transform);
        dustParticleInstance.Stop(); 
    }

    void Update()
    {
        GatherInput();

        if (!isMoving)
        {
            if (TryMove(lastInput)) return; 
            if (TryMove(currentInput)) return; 
        }

        
        if (isMoving && !dustParticleInstance.isPlaying)
        {
            dustParticleInstance.Play(); 
        }
        else if (!isMoving && dustParticleInstance.isPlaying)
        {
            dustParticleInstance.Stop(); 
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
            SetMovementAnimation(direction); 
            StartCoroutine(MoveToPosition(targetPosition));
            hasCollidedWithWall = false; 

            
            if (IsPellet(newPosition))
            {
                DestroyPellet(newPosition);
            }

            return true;
        }
        else
        {
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
        return tile == null; 
    }

    void HandleWallCollision()
    {
        
        audioSource.PlayOneShot(wallCollisionSound);

        
        if (!hasCollidedWithWall)
        {
            TriggerWallCollisionEffect(transform.position);
            hasCollidedWithWall = true; 
        }
    }

    void TriggerWallCollisionEffect(Vector3 position)
    {
        ParticleSystem wallEffect = Instantiate(wallCollisionParticlePrefab, position, Quaternion.identity);
        wallEffect.Play();
        Destroy(wallEffect.gameObject, 1f); 
    }

    void SetMovementAnimation(KeyCode direction)
    {
        animator.SetBool("isMoving", true);
        animator.ResetTrigger("moveUp");
        animator.ResetTrigger("moveDown");
        animator.ResetTrigger("moveLeft");
        animator.ResetTrigger("moveRight");

        
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

        
        animator.SetBool("isMoving", false);
        audioSource.Stop();
    }

    bool IsPellet(Vector3 position)
    {
        
        TileBase pelletTile = pelletTilemap.GetTile(pelletTilemap.WorldToCell(position));
        return pelletTile != null;
    }

    void DestroyPellet(Vector3 position)
    {
        audioSource.PlayOneShot(pelletClip); 

        Vector3Int tilePosition = pelletTilemap.WorldToCell(position);
        pelletTilemap.SetTile(tilePosition, null); 

      
        if (scoreManager != null)
        {
            scoreManager.AddScore(10); 
        }
    }
}



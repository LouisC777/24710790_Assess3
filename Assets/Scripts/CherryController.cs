using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab; // Assign the cherry prefab in the Inspector
    public float moveSpeed = 5f;
    public float spawnInterval = 10f; // Time interval for spawning the cherry
    private Vector3 centerPosition;

    private void Start()
    {
        // Get the center of the level
        centerPosition = new Vector3(8, -10, 0); // Replace with actual center coordinates if needed
        InvokeRepeating(nameof(SpawnCherry), spawnInterval, spawnInterval);
    }

    private void SpawnCherry()
    {
        // Choose a random spawn position outside the camera view
        Vector3 spawnPosition = GetRandomSpawnPositionOutsideCamera();
        
        // Instantiate cherry at the spawn position
        GameObject cherryInstance = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);

        // Start the cherry moving across the level
        StartCoroutine(MoveCherryAcrossLevel(cherryInstance));
    }

    private Vector3 GetRandomSpawnPositionOutsideCamera()
    {
        // Get the camera bounds
        Camera mainCamera = Camera.main;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Randomize spawn position outside camera view on any side
        int side = Random.Range(0, 4); // 0 = Top, 1 = Bottom, 2 = Left, 3 = Right
        Vector3 spawnPosition = Vector3.zero;

        switch (side)
        {
            case 0: // Top
                spawnPosition = new Vector3(Random.Range(-camWidth / 2, camWidth / 2), camHeight / 2 + 1f, 0);
                break;
            case 1: // Bottom
                spawnPosition = new Vector3(Random.Range(-camWidth / 2, camWidth / 2), -camHeight / 2 - 1f, 0);
                break;
            case 2: // Left
                spawnPosition = new Vector3(-camWidth / 2 - 1f, Random.Range(-camHeight / 2, camHeight / 2), 0);
                break;
            case 3: // Right
                spawnPosition = new Vector3(camWidth / 2 + 1f, Random.Range(-camHeight / 2, camHeight / 2), 0);
                break;
        }

        return spawnPosition;
    }

    private System.Collections.IEnumerator MoveCherryAcrossLevel(GameObject cherry)
    {
        Vector3 startPosition = cherry.transform.position;
        Vector3 endPosition = centerPosition * 2 - startPosition; // Reflecting position across the center to get opposite side

        float elapsedTime = 0;
        float journeyLength = Vector3.Distance(startPosition, endPosition);

        // Move the cherry from one side of the screen to the other
        while (elapsedTime < journeyLength / moveSpeed)
        {
            cherry.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime * moveSpeed) / journeyLength);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy cherry once it exits the camera view
        Destroy(cherry);
    }
}

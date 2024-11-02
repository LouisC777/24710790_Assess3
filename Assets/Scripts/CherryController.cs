using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab; 
    public float moveSpeed = 5f;
    public float spawnInterval = 10f; 
    private Vector3 centerPosition;

    private void Start()
    {
        
        centerPosition = new Vector3(8, -10, 0); 
        InvokeRepeating(nameof(SpawnCherry), spawnInterval, spawnInterval);
    }

    private void SpawnCherry()
    {
        
        Vector3 spawnPosition = GetRandomSpawnPositionOutsideCamera();
        
        
        GameObject cherryInstance = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);

        
        StartCoroutine(MoveCherryAcrossLevel(cherryInstance));
    }

    private Vector3 GetRandomSpawnPositionOutsideCamera()
    {
        
        Camera mainCamera = Camera.main;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        
        int side = Random.Range(0, 4); 
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
        Vector3 endPosition = centerPosition * 2 - startPosition; 

        float elapsedTime = 0;
        float journeyLength = Vector3.Distance(startPosition, endPosition);

       
        while (elapsedTime < journeyLength / moveSpeed)
        {
            cherry.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime * moveSpeed) / journeyLength);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

       
        Destroy(cherry);
    }
}

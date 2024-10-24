using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelMap : MonoBehaviour
{
    public Tilemap topLeftTilemap;
    public Tilemap topRightTilemap;
    public Tilemap bottomLeftTilemap;
    public Tilemap bottomRightTilemap;

    private int[,] grid; // Array to hold the grid layout

    void Start()
    {
        CreateGrids();
    }

    void CreateGrids()
    {
        // Define grid sizes for each tilemap
        CreateGrid(topLeftTilemap, 0, 1); // Quadrant 1
        CreateGrid(topRightTilemap, 1, 1); // Quadrant 2
        CreateGrid(bottomLeftTilemap, 0, 0); // Quadrant 3
        CreateGrid(bottomRightTilemap, 1, 0); // Quadrant 4
    }

    void CreateGrid(Tilemap tilemap, int quadrantX, int quadrantY)
    {
        BoundsInt bounds = tilemap.cellBounds; // Get the bounds of the tilemap
        int[,] localGrid = new int[bounds.size.x, bounds.size.y]; // Local grid for this tilemap

        // Loop through each cell in the tilemap
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x + bounds.x, y + bounds.y, 0);
                TileBase tile = tilemap.GetTile(cellPosition); // Get the tile at the current cell

                // Define the grid value based on the presence of a tile
                localGrid[x, y] = (tile != null) ? 0 : 1; // 0 = wall (not walkable), 1 = walkable
            }
        }

        // Merge local grid into the main grid based on the quadrant
        int offsetX = quadrantX * bounds.size.x; // Offset for x based on quadrant
        int offsetY = quadrantY * bounds.size.y; // Offset for y based on quadrant

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                grid[x + offsetX, y + offsetY] = localGrid[x, y];
            }
        }
    }

    // Method to check if a grid position is walkable
    public bool IsWalkable(Vector3Int gridPosition)
    {
        if (gridPosition.x >= 0 && gridPosition.x < grid.GetLength(0) &&
            gridPosition.y >= 0 && gridPosition.y < grid.GetLength(1))
        {
            return grid[gridPosition.x, gridPosition.y] == 1; // Check if the position is walkable
        }
        return false; // Outside bounds is not walkable
    }
}

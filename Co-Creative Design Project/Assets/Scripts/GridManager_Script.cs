using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=kkAjpQAM-jE&ab_channel=Tarodev
public class GridManager_Script : MonoBehaviour
{
    #region Tile Variables
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int enemySpawned = 0, maxEnemies = 2;
    [SerializeField] private Tile_Script tilePrefab;
    private Dictionary<Vector2, Tile_Script> tiles;
    private bool sceneInit = false;
    #endregion

    [SerializeField] private Transform cam; // main camera
    private const int zero = 0, two = 2;

    // Start is called before the first frame update
    void Start()
    {
        //if (GameManager.gMan.roundNo % 5 == 0) // will move to a function later
        //{
        //    maxEnemies++;
        //}
        GenerateGrid();
    }

    void Update()
    {
        if (!sceneInit)
            SpawnEnemies();

        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    //Debug.Log(new Vector2(Mathf.RoundToInt(touchPosition.x), Mathf.RoundToInt(touchPosition.y)));
                    //GetTileAtPosition(new Vector2(Mathf.RoundToInt(touchPosition.x), Mathf.RoundToInt(touchPosition.y))).spriteRenderer.color = Color.red;
                    break;
            }
        }
    }

    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile_Script>();
        GameObject parentGrid = new GameObject("Grid"); // creates parent gameobject

        // Create Grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity); // create tile
                spawnedTile.name = $"Tile {x} {y}"; // set tile name
                spawnedTile.transform.parent = parentGrid.transform; // sets 'Grid' as the parent

                var isOffset = (x % two == zero && y % two != zero) || (x % two != zero && y % two == zero); // is 'x' even and 'y' odd or is 'y' even and 'x' odd?
                spawnedTile.SetColour(isOffset);
                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        cam.transform.position = new Vector3((float)width / two - 0.5f, (float)height / two - 0.5f, -10); // set the grid relative to the camera position
    }

    public void SpawnEnemies()
    {
        if (enemySpawned == maxEnemies)
            sceneInit = true;

        while (enemySpawned != maxEnemies) // loop continues until the enemy spawned count meets the max count
        {
            // Generate random numbers for enemy spawn
            int randomWidthNo = Random.Range(width/two, width); // enemy spawns only on the right side of the board
            int randomHeightNo = Random.Range(zero, height);

            if (GetTileAtPosition(new Vector2(randomWidthNo, randomHeightNo)).GetComponent<Tile_Script>().isOccupied)
            {
                Debug.LogWarning($"Enemy tried spawning on an already occupied tile (W:{randomWidthNo}, H:{randomHeightNo}). Finding new tile....");
                return;
            }

            Debug.Log($"(W:{randomWidthNo}, H:{randomHeightNo})");
            GetTileAtPosition(new Vector2(randomWidthNo, randomHeightNo)).spriteRenderer.color = Color.red;
            GetTileAtPosition(new Vector2(randomWidthNo, randomHeightNo)).GetComponent<Tile_Script>().isOccupied = true;
            enemySpawned++;
        }
    }

    public Tile_Script GetTileAtPosition(Vector2 pos) // get specific tile to program tile logic
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
}

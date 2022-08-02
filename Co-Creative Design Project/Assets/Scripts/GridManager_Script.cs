using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=kkAjpQAM-jE&ab_channel=Tarodev
public class GridManager_Script : MonoBehaviour
{
    #region Tile Variables
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Tile_Script tilePrefab;
    private Dictionary<Vector2, Tile_Script> tiles;
    [SerializeField] private List<GameObject> tileList = new List<GameObject>();
    #endregion

    #region Enemy Variables
    [SerializeField] private int enemySpawned = 0;
    public int maxEnemies = 2;
    [SerializeField] private GameObject enemyPrefab;
    #endregion

    [SerializeField] private Transform cam; // main camera
    [SerializeField] private bool sceneInit = false;
    private const int zero = 0, two = 2, playerLayer = 9;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid(); // creates the grid
    }

    void Update()
    {
        if (!sceneInit) // spawns enemies on editor 'Play' button (will need to change later)
            SpawnEnemies();

        if (GameManager.gMan.roundStart)
        {
            for (int i = 0; i < tileList.Count; i++)
            {
                tileList[i].GetComponent<BoxCollider2D>().enabled = false;
                tileList[i].GetComponent<Tile_Script>().shrink = true;
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

                #region Setting Player Placement
                // Setting tile layers (restricting player placement to the right side of the board)
                tileList.Add(spawnedTile.gameObject); // make a list and add every spawned tile into said list
                var playerTiles = tileList.GetRange(0, tileList.Count / 2); // divide the list by two
                for (int i = 0; i < playerTiles.Count; i++)
                {
                    playerTiles[i].layer = playerLayer; // change the layer of each tile to 'playerLayer'
                }
                #endregion
            }
        }
        cam.transform.position = new Vector3((float)width / two - 0.5f, (float)height / two - 0.5f, -10); // set the grid relative to the camera position
    }

    public void SpawnEnemies()
    {
        if (enemySpawned == maxEnemies) // this makes sure that this function only happens once on 'Play'
        {
            sceneInit = true;
        }
            
        while (enemySpawned != maxEnemies) // loop continues until the enemy spawned count meets the max enemy count
        {
            // Generate random numbers for enemy spawn
            int randomWidthNo = Random.Range(width/two, width); // enemy spawns only on the right side of the board
            int randomHeightNo = Random.Range(zero, height);

            if (GetTileAtPosition(new Vector2(randomWidthNo, randomHeightNo)).GetComponent<Tile_Script>().isOccupied)
            {
                Debug.LogWarning($"Enemy tried spawning on an already occupied tile (W:{randomWidthNo}, H:{randomHeightNo}). Finding new tile...."); // debug a warning if a enemy is attempting to spawn on an already occupied tile
                return;
            }

            Debug.Log($"(W:{randomWidthNo}, H:{randomHeightNo})");
            Instantiate(enemyPrefab, new Vector2(randomWidthNo, randomHeightNo), Quaternion.identity); // create enemy at tile position
            GetTileAtPosition(new Vector2(randomWidthNo, randomHeightNo)).GetComponent<Tile_Script>().isOccupied = true; // that tile is now 'occupied'
            enemySpawned++; // for every enemy spawn, plus one to this integer
        }
    }

    public void StartNewRound()
    {
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy"); // create an array (*that's not-visable) that contains every enemy tagged with "Enemy"
        // Clear Grid
        foreach (GameObject item in enemyArray)
        {
            item.GetComponent<Enemy_Script>().occupiedTile.GetComponent<Tile_Script>().isOccupied = false; // get tile through the enemy and tile is no longer occupied
            item.GetComponent<Enemy_Script>().occupiedTile.SetActive(true); // re-activated said tile in the scene
            Destroy(item); // destroy the enemy
        }
        enemySpawned = 0; // "enemySpawned" is reset to 0
        sceneInit = false;
        SpawnEnemies(); // spawn the enemies again!
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

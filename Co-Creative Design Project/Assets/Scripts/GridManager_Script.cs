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
    #endregion

    #region Enemy Variables
    [SerializeField] private int enemySpawned = 0;
    public int maxEnemies = 2;
    [SerializeField] private GameObject enemyPrefab;
    #endregion

    [SerializeField] private Transform cam; // main camera
    [SerializeField] private bool sceneInit = false;
    private const int zero = 0, two = 2;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid(); // creates the grid
    }

    void Update()
    {
        if (!sceneInit) // spawns enemies on editor 'Play' button (will need to change later)
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

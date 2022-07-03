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

    [SerializeField] private Transform cam; // main camera
    private const int two = 2;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
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

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity); // create tile
                spawnedTile.name = $"Tile {x} {y}"; // set tile name
                spawnedTile.transform.parent = parentGrid.transform; // sets 'Grid' as the parent

                var isOffset = (x % two == 0 && y % two != 0) || (x % two != 0 && y % two == 0); // is 'x' even and 'y' odd or is 'y' even and 'x' odd?
                spawnedTile.SetColour(isOffset);
                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        cam.transform.position = new Vector3((float)width/two - 0.5f, (float)height/two - 0.5f, -10); // set the grid relative to the camera position
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

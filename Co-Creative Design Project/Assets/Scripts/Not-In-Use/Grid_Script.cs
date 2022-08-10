using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=waEsGu--9P8&ab_channel=CodeMonkey //7:30
public class Grid_Script 
{
    // tile variables
    private int width; 
    private int height;
    private float cellSize;
    private int[,] gridArray; // multidimensional array

    public Grid_Script(int width, int height, float cellSize) // constructer
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width,height];

        for (int x = 0; x < gridArray.GetLength(0); x++) // cycling through the multidimensional array
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Testing_Script.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 32, Color.yellow, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f); // vertical line
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f); // horizontal line
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f); // horizontal line
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f); // horizontal line
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }
}
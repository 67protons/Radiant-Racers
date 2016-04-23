using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {
    public static Vector2 gridSize = new Vector2(50, 50);
    private static List<List<CellID>> _grid = new List<List<CellID>>();  

    void Awake()
    {
        CreateGrid();        
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            _grid.Add(new List<CellID>());
            for (int y = 0; y < gridSize.y; y++)
            {
                if (x == 0 || x == gridSize.x - 1 || y == 0 || y == gridSize.y - 1)
                {
                    _grid[x].Add(CellID.Wall);
                }
                else
                {
                    _grid[x].Add(CellID.None);
                }
            }
        }
    }

    public static Vector2 GridPosition(Vector2 inVector)
    {        
        return new Vector2(Mathf.FloorToInt(inVector.x), Mathf.FloorToInt(-inVector.y));
    }

    public static CellID GetCell(Vector2 location)
    {
        int x = (int)location.x, y = (int)location.y;

        if (!isValidCell(x, y))
            throw new System.IndexOutOfRangeException("Invalid cell");            
        return _grid[x][y];
    }

    public static void SetCell(Vector2 location, CellID cell)
    {
        int x = (int)location.x, y = (int)location.y;

        if (!isValidCell(x, y))
            return;

        if (_grid[x][y] == CellID.None)
        {
            _grid[x][y] = cell;
        }
    }

    private static bool isValidCell(int x, int y)
    {
        return (0 <= x && x < gridSize.x && 0 <= y && y < gridSize.y);
    }
}

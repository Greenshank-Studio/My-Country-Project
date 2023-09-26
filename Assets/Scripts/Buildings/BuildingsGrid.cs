using System.Collections.Generic;
using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    public static Cell[,] Grid;
    public static Vector2Int GridSize = new (11, 11);

    private void Awake()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        Grid = new Cell[GridSize.x, GridSize.y];
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Grid[j, i] = new(new(j, i));
                Grid[j, i].Type = CellType.Grass;
            }
        }
    }

    public static void CheckAllCells()
    {
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Debug.Log(Grid[j, i].BuildingOnCell);
            }
        }
        
    }

    public static bool IsPositionExist(Vector2Int position)
    {
        if (position.x >= 0 && position.x < GridSize.x && 
            position.y >= 0 && position.y < GridSize.y) 
            return true;

        return false;
    }

    public static List<Cell> GetAdjacentCellsOfType(int x, int y, CellType type)
    {
        List<Cell> adjacentCells = GetAllAdjacentCells(x, y);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (Grid[adjacentCells[i].Position.x, adjacentCells[i].Position.y].Type != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }

    internal static List<Cell> GetAllAdjacentCells(int x, int y)
    {
        List<Cell> adjacentCells = new List<Cell>();
        if (x > 0)
        {
            adjacentCells.Add(new Cell(new Vector2Int(x - 1, y)));
        }
        if (x < GridSize.x - 1)
        {
            adjacentCells.Add(new Cell(new Vector2Int(x + 1, y)));
        }
        if (y > 0)
        {
            adjacentCells.Add(new Cell(new Vector2Int(x, y - 1)));
        }
        if (y < GridSize.y - 1)
        {
            adjacentCells.Add(new Cell(new Vector2Int(x, y + 1)));
        }
        return adjacentCells;
    }
}

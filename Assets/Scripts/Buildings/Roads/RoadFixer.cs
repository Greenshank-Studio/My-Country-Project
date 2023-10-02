using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{
    public static RoadFixer Instance { private set; get; }

    public Road deadEnd, roadStraight, corner, threeWay, fourWay;

    private int recursionStep;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        recursionStep = 0;
    }

    public void ModifyRoad(Cell cell)
    {
        List<Cell> neigbours = cell.GetFourNeighbours();
        var roadNeigbours = neigbours.Where(n => n.Type.Equals(CellType.Road));
        int roadCount = roadNeigbours.Count();
        
        switch (roadCount)
        {
            case 0:
                FixRoadModel(cell, deadEnd, roadNeigbours);
                break;
            case 1:
                FixRoadModel(cell, deadEnd, roadNeigbours);
                break;
            case 2:
                int xDiff = roadNeigbours.ToArray()[0].Position.x - roadNeigbours.ToArray()[1].Position.x;
                int yDiff = roadNeigbours.ToArray()[0].Position.y - roadNeigbours.ToArray()[1].Position.y;

                if (xDiff == 0 || yDiff == 0)
                {
                    FixRoadModel(cell, roadStraight, roadNeigbours);
                }
                else
                {
                    FixRoadModel(cell, corner, roadNeigbours);
                }
                break;
            case 3:
                FixRoadModel(cell, threeWay, roadNeigbours);
                break;
            case 4:
                FixRoadModel(cell, fourWay, roadNeigbours);
                break;
            default:
                FixRoadModel(cell, deadEnd, roadNeigbours);
                break;
        }

        /*if(recursionStep < 2)
        {
            foreach(Cell c in roadNeigbours.ToList())
            {
                ModifyRoad(c);
            }

            recursionStep++;
        }*/
    }

    private void FixRoadModel(Cell cell, Road road, IEnumerable<Cell> neigbours)
    {
        Quaternion quaternion = SetRightRotation(cell, neigbours);

        Structure structure = Instantiate(
                    road,
                    new Vector3(cell.Position.x, 0f, cell.Position.y),
                    quaternion);

        Map.DestroyCell(cell.StructureOnCell);

        cell.StructureOnCell = structure;
        cell.Type = CellType.Road;

        Map.Grid[cell.Position.x, cell.Position.y] = cell;
    }

    private Quaternion SetRightRotation(Cell cell, IEnumerable<Cell> neigbours)
    {
        Quaternion quaternion;
        int count = neigbours.Count();
        
        switch (count)
        {
            case 0:
            case 1:
                quaternion = SetRotationForDeadEnd(cell, neigbours);
                break;
            case 2:
                quaternion = SetRotationForStraightOrCorner(cell, neigbours);
                break;
            case 3:
                quaternion = SetRotationForThreeWay(cell);
                break;
            default:
                quaternion = Quaternion.Euler(0f, 0f, 0f);
                break;
        }
        return quaternion;
    }

    private Quaternion SetRotationForDeadEnd(Cell cell, IEnumerable<Cell> neighbours)
    {
        List<Cell> cells = neighbours.ToList();

        if(cells.Count > 0)
        {
            Vector2Int difference = cell.Position - cells[0].Position;

            if(difference.x > 0) return Quaternion.Euler(0f, 90f, 0f);
            else if(difference.x < 0) return Quaternion.Euler(0f, -90f, 0f);
            else if (difference.y < 0) return Quaternion.Euler(0f, 180f, 0f);
            else if (difference.y > 0) return Quaternion.Euler(0f, 0f, 0f);
        }

        return Quaternion.Euler(0f, 0f, 0f);
    }

    private Quaternion SetRotationForStraightOrCorner(Cell cell, IEnumerable<Cell> neighbours)
    {
        List<Cell> cells = neighbours.ToList();

        Vector2Int previousDifference = cell.Position - cells[0].Position;
        Vector2Int currentDifference = cells[1].Position - cell.Position;
        
        // straight
        if (currentDifference.y == 0 && previousDifference.y == 0)
        {
            return Quaternion.Euler(0f, 90f, 0f);
        }
        if (currentDifference.x == 0 && previousDifference.x == 0)
        {
            return Quaternion.Euler(0f, 0f, 0f);
        }
        
        // corner
        if (currentDifference.y == 1 && previousDifference.x == 1
            || currentDifference.x == -1 && previousDifference.y == -1)
        {
            return Quaternion.Euler(0f, -90f, 0f);
        }
        else if (currentDifference.x == 1 && previousDifference.y == 1
            || currentDifference.y == -1 && previousDifference.x == -1)
        {
            return Quaternion.Euler(0f, 90f, 0f);
        }
        else if (currentDifference.y == -1 && previousDifference.x == 1
            || currentDifference.x == -1 && previousDifference.y == 1)
        {
            return Quaternion.Euler(0f, 180f, 0f);
        }
        else if (currentDifference.y == 1 && previousDifference.x == -1
            || currentDifference.x == 1 && previousDifference.y == -1)
        {
            return Quaternion.Euler(0f, 0f, 0f);
        }

        return Quaternion.Euler(0f, 0f, 0f);
    }

    private Quaternion SetRotationForThreeWay(Cell cell)
    {
        List<Cell> cells = cell.GetFourNeighbours();
        Cell notRoadCell = cell;

        foreach(Cell c in cells)
        {
            Debug.Log(c.Type);
            if(c.Type != CellType.Road)
            {
                notRoadCell = c;
            }
        }

        Vector2Int difference = cell.Position - notRoadCell.Position;
        
        if (difference.x > 0) return Quaternion.Euler(0f, 180f, 0f);
        else if (difference.x < 0) return Quaternion.Euler(0f, 0f, 0f);
        else if (difference.y > 0) return Quaternion.Euler(0f, 90f, 0f);
        else if (difference.y < 0) return Quaternion.Euler(0f, -90f, 0f);

        return Quaternion.Euler(0f, 0f, 0f);
    }
}

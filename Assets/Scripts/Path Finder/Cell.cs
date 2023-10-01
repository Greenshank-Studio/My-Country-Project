using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Cell Parent { get; private set; }
    public Cell Child { get; set; }

    public Vector2Int Position { get; }

    public float Distance { get; private set; }
    public float DistanceLeft { get; set; }

    public Structure StructureOnCell { get; set; }

    private CellType _type;
    public CellType Type
    {
        get
        {
            if (!Map.IsPositionExist(Position))
                return CellType.OutOfRange;

            return _type;
        }
        set { _type = value; }
    }

    public Cell(Vector2Int position)
    {
        Position = position;
        Distance = 0;
        DistanceLeft = 0;
    }

    public Cell(Vector2Int position, Structure cellStructure)
    {
        Position = position;
        StructureOnCell = cellStructure;
        Distance = 0;
        DistanceLeft = 0;
    }

    public Cell SetParent(Cell cell)
    {
        Parent = cell;

        return this;
    }

    public Cell SetDistance(float distance)
    {
        Distance = distance;

        return this;
    }

    public Vector2Int GetPosition()
    {
        return Position;
    }

    public bool IsFreeToMove()
    {
        if (!Map.IsPositionExist(Position)) 
            return false;

        return Map.Grid[Position.x, Position.y].Type switch
        {
            CellType.Grass => true,
            _ => false
        };
    }

    public Cell GetNeighbour(int biasX, int biasY)
    {
        return new Cell(new Vector2Int(Position.x + biasX, Position.y + biasY));
    }

    public List<Cell> GetFourNeighbours()
    {
        List<Cell> cells = new(4);

        for(int x = -1; x < 2; x += 2)
        {
            if (!(Position.x + x < 0 || Position.x + x > Map.GridSize.x - 1!)) 
            {
                cells.Add(Map.Grid[Position.x + x, Position.y]);
            }
        }
        for (int y = -1; y < 2; y += 2)
        {
            if (!(Position.y + y < 0 || Position.y + y > Map.GridSize.y - 1))
            {
                cells.Add(Map.Grid[Position.x, Position.y + y]);
            }
        }
        return cells;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Cell p = (Cell)obj;
            return (Position == p.Position);
        }
    }

    public override int GetHashCode()
    {
        return Position.x * 1000 + Position.y;
    }

    public override string ToString()
    {
        return Position.x + " " + Position.y;
    }
}

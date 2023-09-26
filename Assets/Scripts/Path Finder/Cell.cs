using UnityEngine;

public class Cell
{
    public Cell Parent { get; private set; }
    public Cell Child { get; set; }

    public Vector2Int Position { get; }

    public float Distance { get; private set; }
    public float DistanceLeft { get; set; }

    public Building BuildingOnCell { get; set; }

    private CellType _type;
    public CellType Type
    {
        get
        {
            if (!BuildingsGrid.IsPositionExist(Position))
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

    public Cell(Vector2Int position, Cell parent, float distance)
    {
        Position = position;
        Parent = parent;
        Distance = distance;
        DistanceLeft = 0;
    }

    public Cell(Vector2Int position, Cell parent, float distance, float distanceLeft)
    {
        Position = position;
        Parent = parent;
        Distance = distance;
        DistanceLeft = distanceLeft;
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
        if (!BuildingsGrid.IsPositionExist(Position)) 
            return false;

        return BuildingsGrid.Grid[Position.x, Position.y].Type switch
        {
            CellType.Grass => true,
            _ => false
        };
    }

    public Cell GetNeighbour(int biasX, int biasY)
    {
        return new Cell(new Vector2Int(Position.x + biasX, Position.y + biasY));
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

using System.Collections.Generic;

public class PathFinder
{
    private const float PerpendicularDistance = 1f;
    private const float DiagonalDistance = 1.4f;

    public static Cell Search(Cell entry, Cell target)
    {
        return SearchInWidth(entry, target);
    }

    private static Cell SearchInWidth(Cell entry, Cell target)
    {
        Dictionary<int, Cell> visited = new Dictionary<int, Cell>();
        Queue<Cell> toVisit = new Queue<Cell>();

        entry.DistanceLeft = (target.Position - entry.Position).magnitude;
        toVisit.Enqueue(entry);

        while (toVisit.Count > 0)
        {
            Cell current = toVisit.Dequeue();

            if (current.Equals(target))
            {
                return current;
            }

            visited.Add(current.GetHashCode(), current);
            List<Cell> neighbours = GetNeighbours(current);

            foreach (Cell neighbour in neighbours)
            {
                if (!visited.ContainsKey(neighbour.GetHashCode()) && !toVisit.Contains(neighbour))
                {
                    neighbour.DistanceLeft = (target.Position - neighbour.Position).magnitude;
                    toVisit.Enqueue(neighbour);
                }
            }
        }

        return null;
    }

    private static List<Cell> GetNeighbours(Cell cell, bool addDiagonal = false)
    {
        List<Cell> neighbours = new List<Cell>();

        for (int x = -1; x < 2; x += 2)
        {
            Cell horizontalNeighbour = cell.GetNeighbour(x, 0);

            AddIfFreeToMove(horizontalNeighbour, PerpendicularDistance);

            for (int y = -1; y < 2; y += 2)
            {
                Cell verticalNeighbour = cell.GetNeighbour(0, y);

                if (x == -1)
                    AddIfFreeToMove(verticalNeighbour, PerpendicularDistance);

                if (addDiagonal && horizontalNeighbour.IsFreeToMove() && verticalNeighbour.IsFreeToMove())
                {
                    Cell diagonalNeighbour = cell.GetNeighbour(x, y);
                    AddIfFreeToMove(diagonalNeighbour, DiagonalDistance);
                }
            }
        }

        return neighbours;

        void AddIfFreeToMove(Cell newCell, float distance)
        {
            if (newCell.IsFreeToMove())
                neighbours.Add(newCell.SetParent(cell).SetDistance(cell.Distance + distance));
        }
    }
}

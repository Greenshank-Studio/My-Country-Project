using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBuilding : Building
{
    [SerializeField] private RoadBuilding _straightRoad;
    [SerializeField] private RoadBuilding _turnRoad;

    private Cell _startPoint = new (Vector2Int.one);
    private Cell _currentPoint = new (Vector2Int.one);

    private bool _isBuildingPlacing;

    private Stack<Cell> _way = new ();
    private List<Building> _tempBuildingList = new ();

    public override void PlaceBuilding(bool isAvailableToBuild, int mousePosX, int mousePosY)
    {
        if (isAvailableToBuild)
        {
            SafeDestroyAndClearBuildingList();

            if (!_isBuildingPlacing && Input.GetMouseButtonDown(1))
            {
                _startPoint = new Cell(new Vector2Int(mousePosX, mousePosY));

                _isBuildingPlacing = !_isBuildingPlacing;

                return;
            }

            if(_isBuildingPlacing)
            {
                _currentPoint = new Cell(new Vector2Int(mousePosX, mousePosY));

                Cell target = PathFinder.Search(_startPoint, _currentPoint);

                FillWayStack(target);

                Cell previousCell = new(new Vector2Int(-1, -1));
                foreach (Cell cell in _way)
                {
                    if (previousCell != null)
                    {
                        //определяет как правильно расположить дорогу
                    }
                    InstantiateFlyingBuilding(
                        cell.Position.x, 
                        cell.Position.y);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    foreach (Cell cell in _way)
                    {
                        PlaceBuildingOnGrid(cell.Position.x, cell.Position.y, CellType.Road);
                    }

                    foreach (Building building in _tempBuildingList)
                    {
                        building.SetNormalColor();
                    }

                    _isBuildingPlacing = !_isBuildingPlacing;

                    Destroy(BuildingsGrid.FlyingBuilding.gameObject);
                }
            }
        }
    }

    private void SafeDestroyAndClearBuildingList()
    {
        if (_tempBuildingList.Capacity > 0)
        {
            foreach (Building building in _tempBuildingList)
            {
                Destroy(building.gameObject);
            }

            _tempBuildingList.Clear();
        }
    }

    private void FillWayStack(Cell target)
    {
        if (target != null)
        {
            _way.Clear();

            while (target.Parent != null)
            {
                _way.Push(target);
                target = target.Parent;
            }

            _way.Push(_startPoint);
        }
    }

    private void InstantiateFlyingBuilding(int posX, int posY)
    {
        Building building = Instantiate(
                        BuildingsGrid.BuildingPrefab,
                        new Vector3(posX, 0f, posY),
                        Quaternion.identity);

        building.SetDisplacementColor(true);

        _tempBuildingList.Add(building);
    }

    /*public void PlaceBuildingAsDrawing(bool isAvailableToBuild, int mousePosX, int mousePosY)
    {
        if (Input.GetMouseButtonUp(1))
        {
            Destroy(BuildingsGrid.FlyingBuilding.gameObject);
        }
        if (isAvailableToBuild)
        {
            if (Input.GetMouseButton(1))
            {
                for (int x = 0; x < Size.x; x++)
                {
                    for (int y = 0; y < Size.y; y++)
                    {
                        BuildingsGrid.Grid[mousePosX + x, mousePosY + y] = CellType.Building;
                    }
                }

                SetNormalColor();

                BuildingsGrid.FlyingBuilding = Instantiate(
                    BuildingsGrid.BuildingPrefab,
                    new Vector3(mousePosX, 0f, mousePosY),
                    Quaternion.identity);
            }
        }
    }*/
}
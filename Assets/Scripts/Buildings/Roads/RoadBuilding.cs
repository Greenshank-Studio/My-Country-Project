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

    public override void PlaceBuilding(ref bool isAvailableToBuild, int mousePosX, int mousePosY)
    {
        if (isAvailableToBuild)
        {
            SafeDestroyAndClearBuildingList();

            // Первое нажатие ЛКМ
            if (!_isBuildingPlacing && Input.GetMouseButtonDown(1))
            {
                _startPoint = new Cell(new Vector2Int(mousePosX, mousePosY));

                _isBuildingPlacing = !_isBuildingPlacing;

                return;
            }

            // Процесс построения дороги (между первым и вторым нажатием ЛКМ)
            if(_isBuildingPlacing)
            {
                _currentPoint = new Cell(new Vector2Int(mousePosX, mousePosY));

                Cell target = PathFinder.Search(_startPoint, _currentPoint);

                FillPathWithCells(target);
                 
                Cell prePreviousCell = _way.Peek();
                Cell previousCell = prePreviousCell;
                Cell currentCell = previousCell;
                
                foreach (Cell cell in _way)
                {
                    prePreviousCell = previousCell;
                    previousCell = currentCell;
                    currentCell = cell;

                    bool isRoadTurn = IsRoadTurn(currentCell, previousCell, prePreviousCell, out float rotationaAngle);

                    InstantiateFlyingBuilding(
                            isRoadTurn ? _turnRoad : _straightRoad,
                            previousCell.Position.x,
                            previousCell.Position.y,
                            rotationaAngle);
                }

                Building lastFlyingRoad = PlaceLastRoadInRightAngle(previousCell, currentCell);

                if(DoObstaclesInterfereWithThePlacementOfLastRoad(lastFlyingRoad))
                {
                    return;
                }
                

                //Второе нажатие ЛКМ
                if (Input.GetMouseButtonDown(1))
                {
                    
                    foreach (Cell cell in _way)
                    {
                        PlaceBuildingOnGrid(cell.Position.x, cell.Position.y, CellType.Road);
                    }
                    
                    foreach (Building building in _tempBuildingList)
                    {
                        SetBuldingOnCell(building);
                        building.SetNormalColor();
                    }
                    
                    Destroy(_tempBuildingList[0].gameObject);
                    Destroy(PlacementManager.FlyingBuilding.gameObject);
                    // Доделать сохранение здания/дороги в клетку на сетке!
                    BuildingsGrid.CheckAllCells();
                }
            }
        }
    }

    private void SafeDestroyAndClearBuildingList()
    {
        if (_tempBuildingList.Count > 0)
        {
            foreach (Building building in _tempBuildingList)
            {
                Destroy(building.gameObject);
            }

            _tempBuildingList.Clear();
        }
    }

    private void FillPathWithCells(Cell target)
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

    private bool IsRoadTurn(Cell cell, Cell previousCell, Cell prePreviousCell, out float rotationAngle)
    {
        Vector2Int prePreviousCellPos = prePreviousCell.GetPosition();
        Vector2Int previousCellPos = previousCell.GetPosition();
        Vector2Int currentCellPos = cell.GetPosition();

        if (prePreviousCellPos.Equals(previousCellPos)
            || previousCellPos.Equals(currentCellPos))
        {
            rotationAngle = 0;
            return false;
        }

        Vector2Int previousPosDifference = previousCellPos - prePreviousCellPos;
        Vector2Int currentPosDiffernce = currentCellPos - previousCellPos;

        bool isTurn = RightRoadTurnCheck(previousPosDifference, currentPosDiffernce, out float angle);
        rotationAngle = angle;

        return isTurn;
    }
    
    private bool RightRoadTurnCheck(Vector2Int previousPosDifference, Vector2Int currentPosDifference, out float angle)
    {
        if(currentPosDifference.y == 0 && previousPosDifference.y == 0)
        {
            angle = 90;
            return false;
        }
        else if (currentPosDifference.x == 0 && previousPosDifference.x == 0)
        {
            angle = 0;
            return false;
        }

        if(currentPosDifference.y == 1 && previousPosDifference.x == 1
            || currentPosDifference.x == -1 && previousPosDifference.y == -1)
        {
            angle = -90;
            return true;
        }
        else if (currentPosDifference.x == 1 && previousPosDifference.y == 1
            || currentPosDifference.y == -1 && previousPosDifference.x == -1)
        {
            angle = 90;
            return true;
        }
        else if (currentPosDifference.y == -1 && previousPosDifference.x == 1
            || currentPosDifference.x == -1 && previousPosDifference.y == 1)
        {
            angle = 180;
            return true;
        }
        else if (currentPosDifference.y == 1 && previousPosDifference.x == -1
            || currentPosDifference.x == 1 && previousPosDifference.y == -1)
        {
            angle = 0;
            return true;
        }

        angle = 0;
        return false;
    }

    private Building PlaceLastRoadInRightAngle(Cell previousCell, Cell currentCell)
    {
        float lastRoadAngle = (currentCell.Position - previousCell.Position).x == 0 ? 0 : 90;

        PlacementManager.FlyingBuilding.transform.eulerAngles = new Vector3(0f, lastRoadAngle, 0f);

        return InstantiateFlyingBuilding(
                    _straightRoad,
                    currentCell.Position.x,
                    currentCell.Position.y,
                    lastRoadAngle);
    }

    private bool DoObstaclesInterfereWithThePlacementOfLastRoad(Building lastFlyingRoad)
    {
        if (!PlacementManager.FlyingBuilding.gameObject.transform.position
                    .Equals(lastFlyingRoad.gameObject.transform.position))
        {
            PlacementManager.FlyingBuilding.SetDisplacementColor(false);
            return true;
        }

        return false;
    }

    private Building InstantiateFlyingBuilding(Building prefab, int posX, int posY, float rotationAngle)
    {
        Building building = Instantiate(
                        prefab,
                        new Vector3(posX, 0f, posY),
                        Quaternion.identity);

        building.transform.eulerAngles = new Vector3(0f, rotationAngle, 0f);
        building.SetDisplacementColor(true);

        _tempBuildingList.Add(building);

        return building;
    }
}
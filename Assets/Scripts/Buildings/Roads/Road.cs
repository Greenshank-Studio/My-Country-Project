using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Road : Structure
{
    [SerializeField] private Road _straightRoad;
    [SerializeField] private Road _turnRoad;

    private Cell _startPoint = new (Vector2Int.one);
    private Cell _currentPoint = new (Vector2Int.one);

    private bool _isRoadPlacing;

    private Stack<Cell> _path = new ();
    private List<Structure> _tempRoadList = new ();

    public override void PlaceStructure(ref bool isAvailableToBuild, int mousePosX, int mousePosY)
    {
        if (isAvailableToBuild)
        {
            ClearCellStructuresOnPath();
            ClearTempRoadList();

            // Первое нажатие ПКМ
            if (!_isRoadPlacing && Input.GetMouseButtonDown(1))
            {
                _startPoint = new Cell(new Vector2Int(mousePosX, mousePosY));

                _isRoadPlacing = !_isRoadPlacing;

                return;
            }

            // Процесс построения дороги (между первым и вторым нажатием ПКМ)
            if(_isRoadPlacing)
            {
                _currentPoint = new Cell(new Vector2Int(mousePosX, mousePosY));

                Cell target = PathFinder.Search(_startPoint, _currentPoint);

                FillPathWithCells(target);
                 
                Cell prePreviousCell = _path.Peek();
                Cell previousCell = prePreviousCell;
                Cell currentCell = previousCell;
                
                /*while(_path.Count != 0)
                {
                    prePreviousCell = previousCell;
                    previousCell = currentCell;
                    currentCell = _path.Pop();

                    bool isRoadTurn = IsRoadTurn(currentCell, previousCell, prePreviousCell, out float rotationaAngle);

                    if (!previousCell.Equals(currentCell))
                        previousCell.StructureOnCell = InstantiateFlyingRoad(
                                isRoadTurn ? _turnRoad : _straightRoad,
                                previousCell.Position.x,
                                previousCell.Position.y,
                                rotationaAngle);
                } */
                
                foreach (Cell cell in _path)
                {
                    prePreviousCell = previousCell;
                    previousCell = currentCell;
                    currentCell = cell;

                    bool isRoadTurn = IsRoadTurn(currentCell, previousCell, prePreviousCell, out float rotationaAngle);

                    if(!previousCell.Equals(currentCell))
                    previousCell.StructureOnCell = InstantiateFlyingRoad(
                            isRoadTurn ? _turnRoad : _straightRoad,
                            previousCell.Position.x,
                            previousCell.Position.y,
                            rotationaAngle);
                }

                Structure lastFlyingRoad = PlaceLastRoadInRightAngle(previousCell, currentCell);

                if (DoObstaclesInterfereWithThePlacementOfRoad(lastFlyingRoad))
                {
                    return;
                }

                //Второе нажатие ПКМ
                if (Input.GetMouseButtonDown(1))
                {
                    foreach (Cell cell in _path)
                    {
                        PlaceCellOnMap(cell.Position.x, cell.Position.y, cell, CellType.Road);
                    }

                    foreach (Structure road in _tempRoadList)
                    {
                        SetStructureOnMap(road);
                        road.SetNormalColor();
                    }

                    Destroy(StructurePlacement.FlyingStructure.gameObject);

                    foreach (Cell cell in _path)
                    {
                        //Debug.Log(cell.Position + " " + cell.StructureOnCell.transform.position);
                        RoadFixer.Instance.ModifyRoad(cell);
                    }
                    
                    //Map.CheckAllCells();
                }
            }
        }
    }

    private void ClearCellStructuresOnPath()
    {
        if (_path.Count > 0)
        {
            foreach (Cell cell in _path)
            {
                cell.StructureOnCell = null;
            }
        }
    }

    private void ClearTempRoadList()
    {
        if (_tempRoadList.Count > 0)
        {
            foreach (Structure road in _tempRoadList)
            {
                Destroy(road.gameObject);
            }

            _tempRoadList.Clear();
        }
    }

    private void FillPathWithCells(Cell target)
    {
        if (target != null)
        {
            _path.Clear();
            
            while (target.Parent != null)
            {
                _path.Push(target);
                target = target.Parent;
            }
            _path.Push(_startPoint);
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

    private Structure PlaceLastRoadInRightAngle(Cell previousCell, Cell currentCell)
    {
        float lastRoadAngle = (currentCell.Position - previousCell.Position).x == 0 ? 0 : 90;

        StructurePlacement.FlyingStructure.transform.eulerAngles = new Vector3(0f, lastRoadAngle, 0f);

        return InstantiateFlyingRoad(
                    _straightRoad,
                    currentCell.Position.x,
                    currentCell.Position.y,
                    lastRoadAngle); ;
    }

    private Structure InstantiateFlyingRoad(Structure prefab, int posX, int posY, float rotationAngle)
    {
        Structure road = Instantiate(
                        prefab,
                        new Vector3(posX, 0f, posY),
                        Quaternion.identity);

        road.transform.eulerAngles = new Vector3(0f, rotationAngle, 0f);
        road.SetDisplacementColor(true);

        _tempRoadList.Add(road);

        return road;
    }

    private bool DoObstaclesInterfereWithThePlacementOfRoad(Structure lastFlyingRoad)
    {
        if (!StructurePlacement.FlyingStructure.gameObject.transform.position
                    .Equals(lastFlyingRoad.gameObject.transform.position))
        {
            StructurePlacement.FlyingStructure.SetDisplacementColor(false);
            return true;
        }

        return false;
    }
}
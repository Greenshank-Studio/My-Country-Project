using System.Collections.Generic;
using UnityEngine;

public class RoadPlacement : StructureManager
{
    public List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();
    public List<Vector3Int> roadPositionsToRecheck = new List<Vector3Int>();

    private Vector3Int startPosition;
    private bool placementMode = false;

    public RoadFixer roadFixer;
    private Vector2Int _roadSize;

    private void Awake()
    {
        roadFixer = GetComponent<RoadFixer>();
        _roadSize = new Vector2Int(1, 1);
    }

    public override void InstantiateFlyingBuilding()
    {
        if (_flyingBuilding != null)
        {
            Destroy(_flyingBuilding.gameObject);
        }

        _flyingBuilding = Instantiate(roadFixer.deadEnd);
        _flyingBuildingRenderer = _flyingBuilding.transform.GetChild(0).GetComponent<MeshRenderer>();
        IsStructureFlying(true);
    }

    protected override bool CheckPositionBeforePlacement(Vector3Int position)
    {
        Vector2Int size = _roadSize;

        if (placementManager.CheckIfPositionInBound(position, size) == false)
        {
            return false;
        }
        if (placementManager.CheckIfPositionIsFree(position, size) == false)
        {
            return false;
        }
        return true;
    }

    public void PlaceRoad(Vector3Int position)
    {
        if(_flyingBuilding != null)
        {
            DestroyFlyingBuilding();
        }

        if (placementManager.CheckIfPositionInBound(position, _roadSize) == false)
            return;
        if (placementManager.CheckIfPositionIsFree(position, _roadSize) == false)
            return;
        if (placementMode == false)
        {
            temporaryPlacementPositions.Clear();
            roadPositionsToRecheck.Clear();

            placementMode = true;
            startPosition = position;

            temporaryPlacementPositions.Add(position);
            placementManager.PlaceTemporaryStructure(position, roadFixer.deadEnd, CellType.Road);

        }
        else
        {
            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPositions.Clear();

            foreach (var positionsToFix in roadPositionsToRecheck)
            {
                roadFixer.FixRoadAtPosition(placementManager, positionsToFix);
            }

            roadPositionsToRecheck.Clear();

            temporaryPlacementPositions = placementManager.GetPathBetween(startPosition, position);

            foreach (var temporaryPosition in temporaryPlacementPositions)
            {
                if (placementManager.CheckIfPositionIsFree(temporaryPosition, _roadSize) == false)
                {
                    roadPositionsToRecheck.Add(temporaryPosition);
                    continue;
                }  
                placementManager.PlaceTemporaryStructure(temporaryPosition, roadFixer.deadEnd, CellType.Road);
            }
        }

        FixRoadPrefabs();

    }

    private void FixRoadPrefabs()
    {
        foreach (var temporaryPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosition(placementManager, temporaryPosition);
            var neighbours = placementManager.GetNeighboursOfTypeFor(temporaryPosition, CellType.Road);
            foreach (var roadposition in neighbours)
            {
                if (roadPositionsToRecheck.Contains(roadposition)==false)
                {
                    roadPositionsToRecheck.Add(roadposition);
                }
            }
        }
        foreach (var positionToFix in roadPositionsToRecheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
        }
    }

    public void FinishPlacingRoad()
    {
        placementMode = false;
        placementManager.AddtemporaryStructuresToStructureDictionary();
        /*if (temporaryPlacementPositions.Count > 0)
        {
            AudioPlayer.instance.PlayPlacementSound();
        }*/
        temporaryPlacementPositions.Clear();
        startPosition = Vector3Int.zero;

        InstantiateFlyingBuilding();
    }
}

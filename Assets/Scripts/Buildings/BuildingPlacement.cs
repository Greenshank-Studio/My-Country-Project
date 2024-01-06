using System;
using UnityEngine;

public class BuildingPlacement : StructureManager
{
    [SerializeField] private StructureReferences[] _structurePrefabs;
    // [SerializeField] private BuildingPlacement _structureManager;
    [SerializeField] private InputManager _inputManager;

    private void Awake()
    {
        _buildingIndex = -1;
    }
    
    public override void InstantiateFlyingBuilding()
    {
        if (_flyingBuilding != null)
        {
            Destroy(_flyingBuilding.gameObject);
        }

        _flyingBuilding = Instantiate(_structurePrefabs[_buildingIndex].Prefab);
        _flyingBuildingRenderer = _flyingBuilding.transform.GetChild(0).GetComponent<MeshRenderer>();
        IsStructureFlying(true);
    }

    public void PlaceHouse(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            placementManager.PlaceObjectOnTheMap(position, _structurePrefabs[_buildingIndex].Size, _structurePrefabs[_buildingIndex].Prefab, CellType.Structure);
            this.DestroyFlyingBuilding();
            _inputManager.OnMouseDown = null;
            _inputManager.OnMouseHold = null;
            _inputManager.OnMouseUp = null;
            _inputManager.OnMouseHover = null;
            // DestroyFlyingBuilding();
            // IsStructureFlying(false);
            //AudioPlayer.instance.PlayPlacementSound();
        }
    }

    protected override bool CheckPositionBeforePlacement(Vector3Int position)
    {
        Vector2Int size = _structurePrefabs[_buildingIndex].Size;

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
}

[Serializable]
public struct StructureReferences
{
    public GameObject Prefab;
    public Vector2Int Size;
}

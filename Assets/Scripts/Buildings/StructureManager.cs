using UnityEngine;

public abstract class StructureManager : MonoBehaviour
{
    [SerializeField] protected PlacementManager placementManager;
    
    protected int _buildingIndex;
    protected GameObject _flyingBuilding;
    protected MeshRenderer _flyingBuildingRenderer;

    private bool _isStructureFlying;
    
    private void Awake()
    {
        _buildingIndex = -1;
    }

    public void SetBuildingIndex(int index)
    {
        _buildingIndex = index;
    }

    public void SetFlyingStructure(Vector3Int position)
    {
        if (_isStructureFlying)
        {
            _flyingBuilding.transform.position = position;
            SetDisplacementColor(position);
        }
    }

    private void SetDisplacementColor(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            _flyingBuildingRenderer.material.color = Color.green;
        }
        else
        {
            _flyingBuildingRenderer.material.color = Color.red;
        }
    }

    public virtual void InstantiateFlyingBuilding()
    {
        if (_flyingBuilding != null)
        {
            Destroy(_flyingBuilding.gameObject);
        }

        // _flyingBuilding = Instantiate(_structurePrefabs[_buildingIndex].Prefab);
        _flyingBuildingRenderer = _flyingBuilding.transform.GetChild(0).GetComponent<MeshRenderer>();
        IsStructureFlying(true);
    }

    public void DestroyFlyingBuilding()
    {
        if (_flyingBuilding != null)
        {
            Destroy(_flyingBuilding.gameObject);
            IsStructureFlying(false);
            // InputManager.OnMouseHover -= BuildingPlacement.SetFlyingStructure;
            // InputManager.OnMouseDown -= BuildingPlacement.PlaceHouse;
        }
    }

    public void IsStructureFlying(bool isFlying)
    {
        _isStructureFlying = isFlying;
    }

    protected virtual bool CheckPositionBeforePlacement(Vector3Int position)
    {
        Vector2Int size = new Vector2Int(1, 1);

        if (placementManager.CheckIfPositionInBound(position, size) == false)
        {
            Debug.Log("This position is out of bounds");
            return false;
        }
        if (placementManager.CheckIfPositionIsFree(position, size) == false)
        {
            Debug.Log("This position is not EMPTY");
            return false;
        }
        return true;
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RoadPlacement _roadManager; // add word "System" to all hidden system objects
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private UIController _uiController;
    [SerializeField] private BuildingPlacement _buildingPlacement;
    [SerializeField] private StructureDeleting _structureDeleting;
    [SerializeField] private CameraMovement _cameraMovement;
    
    [SerializeField] private GameObject _chooseBuildingMenu;

    private void OnEnable() // this method activates when object is enabled
    {
        _uiController.OnRoadPlacement += RoadPlacementHandler; // subscribe to the method
        _uiController.OnHousePlacement += HousePlacementHandler;
        _uiController.OnStructureDelete += DeleteStructureHandler;
    }

    private void OnDisable() // this method activates when object is disabled
    {
        _uiController.OnRoadPlacement -= RoadPlacementHandler;
        _uiController.OnHousePlacement -= HousePlacementHandler;
        _uiController.OnStructureDelete -= DeleteStructureHandler;
    }

    // при нажатии на кнопку House вызывается эта функция, куда передается индекс здания
    private void HousePlacementHandler(int houseIndex)
    {
        ClearInputActions();
        _buildingPlacement.SetBuildingIndex(houseIndex);
        _buildingPlacement.InstantiateFlyingBuilding();
        _inputManager.OnMouseHover += _buildingPlacement.SetFlyingStructure;
        _inputManager.OnMouseDown += _buildingPlacement.PlaceHouse;
    }

    private void RoadPlacementHandler(int roadIndex)
    {
        ClearInputActions();

        _roadManager.SetBuildingIndex(roadIndex);
        _roadManager.InstantiateFlyingBuilding();
        _inputManager.OnMouseHover += _roadManager.SetFlyingStructure;
        _inputManager.OnMouseDown += _roadManager.PlaceRoad;
        _inputManager.OnMouseHold += _roadManager.PlaceRoad;
        _inputManager.OnMouseUp += _roadManager.FinishPlacingRoad;
    }

    private void DeleteStructureHandler()
    {
        ClearInputActions();
        
        _inputManager.OnMouseDown += _structureDeleting.DeleteStructure;
    }

    private void ClearInputActions()
    {
        _buildingPlacement.DestroyFlyingBuilding();
        _roadManager.DestroyFlyingBuilding();

        _inputManager.OnMouseDown = null;
        _inputManager.OnMouseHold = null;
        _inputManager.OnMouseUp = null;
        _inputManager.OnMouseHover = null;
    }
}

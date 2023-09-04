using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    public static Building BuildingPrefab;
    public static Building FlyingBuilding;
    public static CellType[,] Grid;
    public static Vector2Int GridSize = new (11, 11);

    private Camera _mainCamera;

    private void Awake()
    {
        Grid = new CellType[GridSize.x, GridSize.y];

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if(FlyingBuilding != null)
        {
            StartPlacingBuildingProcess();
        }
    }

    private void StartPlacingBuildingProcess()
    {
        Plane groundPlane = new(Vector3.up, Vector3.zero);

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float enterPosition))
        {
            Vector3 worldPosition = ray.GetPoint(enterPosition);

            int mousePosX = Mathf.RoundToInt(worldPosition.x);
            int mousePosY = Mathf.RoundToInt(worldPosition.z);

            bool isAvailableToBuild = IsBuildingCanBePlaced(mousePosX, mousePosY);

            FlyingBuilding.transform.position = new Vector3(mousePosX, 0f, mousePosY);
            FlyingBuilding.SetDisplacementColor(isAvailableToBuild);

            if(Input.GetKeyDown(KeyCode.R)) FlyingBuilding.Flip();

            FlyingBuilding.PlaceBuilding(isAvailableToBuild, mousePosX, mousePosY);
        }
    }

    public void InstantiateNewFlyingBuilding(Building buildingPrefab)
    {
        if (FlyingBuilding != null) 
            Destroy(FlyingBuilding.gameObject);

        BuildingPrefab = buildingPrefab;
        FlyingBuilding = Instantiate(buildingPrefab);
    }

    private bool IsBuildingCanBePlaced(int x, int y)
    {
        if (IsOutOfBounds(x, y) || IsPlaceTaken(x, y)) return false;
        return true;
    }

    private bool IsOutOfBounds(int x, int y)
    {
        if (x < 0 || x > GridSize.x - FlyingBuilding.Size.x
        || y < 0 || y > GridSize.y - FlyingBuilding.Size.y) return true;
        return false;
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < FlyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < FlyingBuilding.Size.y; y++)
            {
                if (Grid[placeX + x, placeY + y] != CellType.Grass) return true;
            }
        }

        return false;
    }

    public static bool IsPositionExist(Vector2Int position)
    {
        if (position.x >= 0 && position.x < GridSize.x && 
            position.y >= 0 && position.y < GridSize.y) 
            return true;

        return false;
    }
}

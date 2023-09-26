
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static Building BuildingPrefab;
    public static Building FlyingBuilding;

    protected Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (FlyingBuilding != null)
        {
            StartPlacingProcess();
        }
    }

    public void StartPlacingProcess()
    {
        Plane groundPlane = new(Vector3.up, Vector3.zero);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float enterPosition))
        {
            Vector3 worldPosition = ray.GetPoint(enterPosition);

            int mousePosX = Mathf.RoundToInt(worldPosition.x);
            int mousePosY = Mathf.RoundToInt(worldPosition.z);

            bool isAvailableToBuild = IsBuildingCanBePlaced(mousePosX, mousePosY);

            FlyingBuilding.SetDisplacementColor(isAvailableToBuild);
            FlyingBuilding.transform.position = new Vector3(mousePosX, 0f, mousePosY);
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

    protected bool IsBuildingCanBePlaced(int x, int y)
    {
        if (IsOutOfBounds(x, y) || IsPlaceTaken(x, y)) return false;
        return true;
    }

    private bool IsOutOfBounds(int x, int y)
    {
        if (x < 0 || x > BuildingsGrid.GridSize.x - FlyingBuilding.Size.x
        || y < 0 || y > BuildingsGrid.GridSize.y - FlyingBuilding.Size.y) return true;
        return false;
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < FlyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < FlyingBuilding.Size.y; y++)
            {
                if (BuildingsGrid.Grid[placeX + x, placeY + y].Type != CellType.Grass) return true;
            }
        }

        return false;
    }
}

using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static Structure StructurePrefab;
    public static Structure FlyingStructure;

    protected Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (FlyingStructure != null)
        {
            StartPlacingProcess();
        }
    }

    private void StartPlacingProcess()
    {
        Plane groundPlane = new(Vector3.up, Vector3.zero);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float enterPosition))
        {
            Vector3 worldPosition = ray.GetPoint(enterPosition);

            int mousePosX = Mathf.RoundToInt(worldPosition.x);
            int mousePosY = Mathf.RoundToInt(worldPosition.z);
            
            bool isAvailableToBuild = IsStructureCanBePlaced(mousePosX, mousePosY);

            FlyingStructure.SetDisplacementColor(isAvailableToBuild);
            FlyingStructure.transform.position = new Vector3(mousePosX, 0f, mousePosY);
            FlyingStructure.PlaceStructure(ref isAvailableToBuild, mousePosX, mousePosY);
        }
    }

    public void InstantiateNewFlyingStructure(Structure structurePrefab)
    {
        if (FlyingStructure != null)
            Destroy(FlyingStructure.gameObject);

        StructurePrefab = structurePrefab;
        FlyingStructure = Instantiate(structurePrefab);
    }

    private bool IsStructureCanBePlaced(int x, int y)
    {
        if (IsOutOfBounds(x, y) || IsPlaceTaken(x, y)) return false;
        return true;
    }

    private bool IsOutOfBounds(int x, int y)
    {
        if (x < 0 || x > Map.GridSize.x - FlyingStructure.Size.x
        || y < 0 || y > Map.GridSize.y - FlyingStructure.Size.y) return true;
        return false;
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < FlyingStructure.Size.x; x++)
        {
            for (int y = 0; y < FlyingStructure.Size.y; y++)
            {
                if (Map.Grid[placeX + x, placeY + y].Type != CellType.Grass) return true;
            }
        }

        return false;
    }
}

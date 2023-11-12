using UnityEngine;

public class StructurePlacement : MonoBehaviour
{
    public static Structure StructurePrefab;
    public static Structure FlyingStructure;

    protected Camera mainCamera;

    public GameObject SelectBuildingMenu;

    public void OpenSelectBuildingMenu(){
        Debug.Log("HELLOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        SelectBuildingMenu.SetActive(true);
        if (SelectBuildingMenu != null){
            SelectBuildingMenu.SetActive(true);
        }
    }

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

    public void InstantiateNewFlyingStructure(Structure structurePrefab)
    {
        if (FlyingStructure != null)
            Destroy(FlyingStructure.gameObject);

        StructurePrefab = structurePrefab;
        FlyingStructure = Instantiate(structurePrefab);
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

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {

    }
}

using UnityEngine;

public class BusinessBuilding : Building, IRotatable
{
    public override void PlaceBuilding(ref bool isAvailableToBuild, int mousePosX, int mousePosY)
    {
        if (isAvailableToBuild)
        {
            if (Input.GetMouseButtonDown(1))
            {
                PlaceBuildingOnGrid(mousePosX, mousePosY, CellType.Building);
                SetBuldingOnCell(PlacementManager.FlyingBuilding);

                SetNormalColor();
                PlacementManager.FlyingBuilding = null;
            }
        }
    }

    public void Rotate()
    {
        buildingModelTransform.localScale = new Vector3(
                buildingModelTransform.localScale.x,
                buildingModelTransform.localScale.y * -1,
                buildingModelTransform.localScale.z);

        buildingModelTransform.Rotate(Vector3.forward, 180f);
    }
}

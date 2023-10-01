using UnityEngine;

public class Building : Structure, IRotatable
{
    public override void PlaceStructure(ref bool isAvailableToBuild, int mousePosX, int mousePosY)
    {
        if (isAvailableToBuild)
        {
            if (Input.GetMouseButtonDown(1))
            {
                PlaceCellOnMap(mousePosX, mousePosY, null, CellType.Building);
                SetStructureOnMap(StructurePlacement.FlyingStructure);

                SetNormalColor();
                StructurePlacement.FlyingStructure = null;
            }
        }
    }

    public void Rotate()
    {
        BuildingModelTransform.localScale = new Vector3(
                BuildingModelTransform.localScale.x,
                BuildingModelTransform.localScale.y * -1,
                BuildingModelTransform.localScale.z);

        BuildingModelTransform.Rotate(Vector3.forward, 180f);
    }
}

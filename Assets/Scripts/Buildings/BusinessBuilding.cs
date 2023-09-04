using System.Collections.Generic;
using UnityEngine;

public class BusinessBuilding : Building
{
    public override void PlaceBuilding(bool isAvailableToBuild, int mousePosX, int mousePosY)
    {
        if(isAvailableToBuild)
        {
            if (Input.GetMouseButtonDown(1))
            {
                PlaceBuildingOnGrid(mousePosX, mousePosY, CellType.Building);

                SetNormalColor();
                BuildingsGrid.FlyingBuilding = null;
            }
        }
    }
}

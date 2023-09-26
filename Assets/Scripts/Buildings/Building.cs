using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public Vector2Int Size = Vector2Int.one;
    public List<Renderer> MainRenderer;

    protected Camera mainCamera;
    protected List<Color> modelColors;
    protected Transform buildingModelTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
        modelColors = new List<Color>();
        buildingModelTransform = transform.GetChild(0).GetComponent<Transform>();

        foreach(Renderer renderer in MainRenderer)
        {
            modelColors.Add(renderer.material.color);
        }
    }

    public virtual void SetDisplacementColor(bool availableToBuild)
    {
        if (availableToBuild)
        {
            foreach (Renderer renderer in MainRenderer)
                renderer.material.color = Color.green;
        }
        else
        {
            foreach (Renderer renderer in MainRenderer)
                renderer.material.color = Color.red;
        }
    }

    public void SetNormalColor()
    {
        for (int i = 0; i < MainRenderer.Capacity; i++)
        {
            MainRenderer[i].material.color = modelColors[i];
        }
    }

    protected void PlaceBuildingOnGrid(int posX, int posY, CellType type)
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                BuildingsGrid.Grid[posX + x, posY + y].Type = type;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0f, y), new Vector3(1f, 0.1f, 1f));
            }
        }
    }

    public abstract void PlaceBuilding(bool isAvailableToBuild, int mousePosX, int mousePosY);
}

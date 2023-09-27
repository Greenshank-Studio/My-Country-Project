using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    [SerializeField] private List<Renderer> MainRenderer;
    public Vector2Int Size = Vector2Int.one;
    
    protected Camera MainCamera;
    protected List<Color> ModelColors;
    protected Transform BuildingModelTransform;

    private void Awake()
    {
        MainCamera = Camera.main;
        ModelColors = new List<Color>();
        BuildingModelTransform = transform.GetChild(0).GetComponent<Transform>();

        foreach(Renderer renderer in MainRenderer)
        {
            ModelColors.Add(renderer.material.color);
        }
    }

    public abstract void PlaceStructure(ref bool isAvailableToBuild, int mousePosX, int mousePosY);

    public void SetDisplacementColor(bool availableToBuild)
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
            MainRenderer[i].material.color = ModelColors[i];
        }
    }

    protected void PlaceStructureOnGrid(int posX, int posY, CellType type)
    {   
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Map.Grid[posX + x, posY + y].Type = type;
            }
        }
    }

    protected void SetStructureOnCell(Structure structure)
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Map.Grid
                    [(int)structure.transform.position.x + x, 
                    (int)structure.transform.position.z + y]
                    .StructureOnCell = structure;
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
}

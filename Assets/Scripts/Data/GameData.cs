using System;
using UnityEngine;

[Serializable]
public class GameData
{
    // currency
    public int Money;

    // grid
    public CellType[,] Grid;
    public Vector2Int GridSize;

    // camera
    public Vector3 CameraPosition;
    public Quaternion CameraRotation;
    public float OrthographicSize;

    public GameData()
    {
        Money = 1000;

        GridSize = new Vector2Int(11, 11);
        Grid = new CellType[GridSize.x, GridSize.y];

        CameraPosition = new Vector3(1.7f, 36f, -63.5f);
        CameraRotation = new Quaternion(30f, 358.4f, 0f, 1f);
        OrthographicSize = 5f;
    }
}


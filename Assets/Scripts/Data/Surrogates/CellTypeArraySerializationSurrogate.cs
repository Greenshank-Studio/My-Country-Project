using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class CellTypeArraySerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var cellTypeArray = (CellType[,]) obj;

        info.AddValue("FirstDimensionLength", cellTypeArray.GetLength(0));
        info.AddValue("SecondDimensionLength", cellTypeArray.GetLength(1));

        for (int i = 0; i < cellTypeArray.GetLength(0); i++)
        {
            for(int j = 0; j < cellTypeArray.GetLength(1); j++)
            {
                info.AddValue($"{i}{j}", cellTypeArray[i, j]);
            }
        }
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        int xSize = (int) info.GetValue("FirstDimensionLength", typeof(int));
        int ySize = (int) info.GetValue("SecondDimensionLength", typeof(int));

        CellType[,] cellTypeArray = new CellType[xSize, ySize];

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                cellTypeArray[i, j] = (CellType) info.GetValue($"{i}{j}", typeof(CellType));
            }
        }

        return cellTypeArray;
    }
}

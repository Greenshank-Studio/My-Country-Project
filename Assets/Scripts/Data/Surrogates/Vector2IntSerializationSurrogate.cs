using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Vector2IntSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var v2 = (Vector2Int) obj;
        info.AddValue("x", v2.x);
        info.AddValue("y", v2.y);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        var v2 = (Vector2Int) obj;
        v2.x = (int) info.GetValue("x", typeof(int));
        v2.y = (int) info.GetValue("y", typeof(int));
        obj = v2;
        return obj;
    }
}

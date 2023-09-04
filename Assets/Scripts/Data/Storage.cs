using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Storage
{
    private string _filePath;
    private BinaryFormatter _formatter;

    public Storage()
    {
        var directory = Application.persistentDataPath + "/saves";
        if(!Directory.Exists(directory)) 
            Directory.CreateDirectory(directory);

        _filePath = directory + "/GameSave.save";
        InitBinaryFormatter();
    }

    private void InitBinaryFormatter()
    {
        _formatter = new BinaryFormatter();
        var selector = new SurrogateSelector();

        var v3Surrogate = new Vector3SerializationSurrogate();
        var v2IntSurrogate = new Vector2IntSerializationSurrogate();
        var quaternionSurrogate = new QuaternionSerializationSurrogate();
        var cellTypeArray = new CellTypeArraySerializationSurrogate();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);
        selector.AddSurrogate(typeof(Vector2Int), new StreamingContext(StreamingContextStates.All), v2IntSurrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);
        selector.AddSurrogate(typeof(CellType[,]), new StreamingContext(StreamingContextStates.All), cellTypeArray);

        _formatter.SurrogateSelector = selector;
    }

    public object Load(object saveDataByDefault)
    {
        if(!File.Exists(_filePath))
        {
            if(saveDataByDefault != null)
            {
                Save(saveDataByDefault);
            }

            return saveDataByDefault;
        }

        var file = File.Open(_filePath, FileMode.Open);
        var savedData = _formatter.Deserialize(file);
        file.Close();
        return savedData;
    }

    public void Save(object saveData)
    {
        var file = File.Create(_filePath);
        _formatter.Serialize(file, saveData);
        file.Close();
    }
}

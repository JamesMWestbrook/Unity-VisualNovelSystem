using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;
public class TestSaveSystem : MonoBehaviour
{
[HideLabel]    public DataSO startData;
public Data data;
    // Start is called before the first frame update
    void Start()
    {


        startData.data.Party.Add(startData.data.PartySO[0].Character);
        startData.data.Party.Add(startData.data.PartySO[1].Character);
        data = startData.data;
        SaveState("Assets/Resources/SaveData/StarterData.bytes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveState(string filePath){
        byte[] bytes = SerializationUtility.SerializeValue(startData.data, DataFormat.Binary);
        File.WriteAllBytes(filePath, bytes);
    }
}

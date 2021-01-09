using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;
using UnityEngine.SceneManagement;
public class TestSaveSystem : MonoBehaviour
{
    [HideLabel] public DataSO startData;
    public Data data;
    // Start is called before the first frame update
    void Start()
    {
        startData.data.Party.Clear();
        for (int i = 0; i < startData.data.PartySO.Count; i++)
        {
            startData.data.Party.Add(startData.data.PartySO[i].Character);
        }

        data = startData.data;
        SaveState("Assets/Resources/SaveData/StarterData.bytes");
        Debug.Log("Saved");
        // SceneManager.LoadScene("GUISetup", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SaveState(string filePath)
    {
        byte[] bytes = SerializationUtility.SerializeValue(startData.data, DataFormat.Binary);
        File.WriteAllBytes(filePath, bytes);
    }
}

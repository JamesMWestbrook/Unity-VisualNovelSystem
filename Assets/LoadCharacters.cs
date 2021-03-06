﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using System.IO;
public class LoadCharacters : MonoBehaviour
{
public      Data newData;
    void Awake()
    {
        LoadData("SaveData/StarterData");
        List<ActorSlot> _actors = GetComponent<BattleManager>().Party;
        Debug.Log(newData.Party.Count);
        _actors[0].Actor = newData.Party[0];
        _actors[1].Actor = newData.Party[1];
        for (int i = 0; i < _actors.Count; i++)
        {
            CharacterBase _curActor = _actors[i].Actor;
            _curActor.Lvl = 1;
            _curActor.StatIncrease();
            Debug.Log("Stat initialization");
            _curActor.CurStats = _curActor.MaxStats.DeepStatsCopy(_curActor.MaxStats);

            _actors[i].SetGraphics();
            _actors[i].UpdateStats();
        }
    }
    void Update()
    {
        
    }
    public void LoadData(string path){
        byte[] bytes = Resources.Load<TextAsset>(path).bytes;
        newData = SerializationUtility.DeserializeValue<Data>(bytes, DataFormat.JSON);
        Debug.Log("dd");
        // newData = SerializationUtility.DeserializeValue<Data>(bytes, DataFormat.Binary);
    }
}

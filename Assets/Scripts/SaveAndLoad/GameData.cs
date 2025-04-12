using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int currency;

    public SerializableDictionary<string, bool> checkpoints;

    public SerializableDictionary<string, bool> skillTree;

    public SerializableDictionary<string, int> inventory;

    public List<string> equip;

    public string closestCheckPointId;
    public GameData()
    {
        skillTree = new SerializableDictionary<string, bool>();
        this.currency = 0;
        equip = new List<string>();
        closestCheckPointId = string.Empty;
        inventory = new SerializableDictionary<string, int>();
        checkpoints= new SerializableDictionary<string, bool>();
    }
}

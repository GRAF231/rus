using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.WeaponData> WeaponData { get; private set; } = new Dictionary<int, Data.WeaponData>();
    public Dictionary<int, Data.Player> PlayerData { get; private set; } = new Dictionary<int, Data.Player>();
    public Dictionary<int, Data.Monster> MonsterData { get; private set; } = new Dictionary<int, Data.Monster>();
    public Dictionary<int, Data.PlayerStatData> PlayerStatData { get; private set; } = new Dictionary<int, Data.PlayerStatData>();

    public void Init()
    {
        PlayerData = LoadJson<Data.PlayerData, int, Data.Player>("PlayerData").MakeDict();
        WeaponData = LoadJson<Data.WeaponDataLoader, int, Data.WeaponData>("WeaponData").MakeDict();
        MonsterData = LoadJson<Data.MonsterData, int, Data.Monster>("MonsterData").MakeDict();
        PlayerStatData = LoadJson<Data.PlayerStatDataLoader, int, Data.PlayerStatData>("PlayerStatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        Debug.Log(textAsset.text);
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

}

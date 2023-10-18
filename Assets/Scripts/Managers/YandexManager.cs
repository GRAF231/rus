using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;

[Serializable]
public class ScopePermissions
{
    public string avatar { get; set; }
    public string public_name { get; set; }
}
[Serializable]
public class Player
{
    public string lang { get; set; }
    public string publicName { get; set; }
    public ScopePermissions scopePermissions { get; set; }
    public string uniqueID { get; set; }
}
[Serializable]
public class Entry
{
    public int score { get; set; }
    public int extraData { get; set; }
    public int rank { get; set; }
    public string formattedScore { get; set; }
    public Player player { get; set; }
}
[Serializable]
public class Leaderboard
{
    public string leaderboardName { get; set; }
}
[Serializable]
public class Range
{
    public int start { get; set; }
    public int size { get; set; }
}
[Serializable]
public class LeaderBoardList
{
    public Leaderboard leaderboard { get; set; }
    public List<Range> ranges { get; set; }
    public int userRank { get; set; }
    public List<Entry> entries { get; set; }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

public class YandexManager : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void GiveMePlayerData();
    [DllImport("__Internal")]
    public static extern void RateGame();
    [DllImport("__Internal")]
    public static extern string GetLang();
    [DllImport("__Internal")]
    public static extern void SetToLeaderBoard(int time, long score);
    [DllImport("__Internal")]
    public static extern string GetLeaderBoard();

    static YandexManager s_instance;
    public static YandexManager Instance { get { Init(); return s_instance; } }

    public event Action OnChangeLeaderBoardList;
    private List<Entry> _leaderBoardList;
    public List<Entry> LeaderBoardList
    {
        get { return _leaderBoardList; }
        set
        {
            _leaderBoardList = value;
            OnChangeLeaderBoardList?.Invoke();
        }
    }

    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("YandexManager");
            if (go == null)
            {
                go = new GameObject { name = "YandexManager" };
                go.AddComponent<YandexManager>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<YandexManager>();
        }
        GetLeaderBoard();
        string lang = GetLang();

        if (lang == "ru")
            Managers.I18n.Lang = I18NManager.Language.ru;
        else
            Managers.I18n.Lang = I18NManager.Language.en;
    }

    public string NameText { get; private set; }
    public RawImage Photo { get; private set; }
    public string Lang { get; private set; }

    public void SetName(string name)
    {
        NameText = name;
    }

    public void SetPhoto(string url)
    {
        StartCoroutine(DownloadImage(url));
    }

    public void BoardEntriesReady(string json)
    {
        var list = JsonConvert.DeserializeObject<LeaderBoardList>(json);
        LeaderBoardList = list.entries;
    }

    IEnumerator DownloadImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
            Photo.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
}
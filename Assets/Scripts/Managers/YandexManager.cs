using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class YandexManager : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void GiveMePlayerData();
    [DllImport("__Internal")]
    public static extern void RateGame();
    [DllImport("__Internal")]
    public static extern string GetLang();
    [DllImport("__Internal")]
    public static extern void SetToLeaderBoard(long score);

    static YandexManager s_instance;
    public static YandexManager Instance { get { Init(); return s_instance; } }

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
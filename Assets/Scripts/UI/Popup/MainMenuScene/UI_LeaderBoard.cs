using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Data;

public class UI_LeaderBoard : UI_Popup
{

    Dictionary<I18NManager.Language, string> localizeType = new Dictionary<I18NManager.Language, string>(2)
    {
        { I18NManager.Language.en, "Perun 's Honor Table" },
        { I18NManager.Language.ru, "Таблица почета Перуна" }
    };
    public override Define.PopupUIGroup _popupID { get { return Define.PopupUIGroup.UI_LeaderBoard; } }

    enum Images
    {
        ProtectImage,
    }

    enum Panels
    {
        TablePanel
    }

    enum Texts
    {
        Title
    }

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(Panels));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetImage((int)Images.ProtectImage).gameObject.AddUIEvent(OnClickDelete);
        Get<TextMeshProUGUI>((int)Texts.Title).text = localizeType[Managers.I18n.Lang];
        SetLeaderBoardData();
        YandexManager.Instance.OnChangeLeaderBoardList += SetLeaderBoardData;
    }

    public void SetLeaderBoardData()
    {
        List<Entry> entries = YandexManager.Instance.LeaderBoardList;
        Debug.Log(entries.Count);
        GameObject tablePanel = GetObject((int)Panels.TablePanel).gameObject;
        foreach (Transform child in tablePanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        foreach (Entry item in entries)
        {
            GameObject go = Managers.Resource.Instantiate("UI/SubItem/ScoreItem", parent: tablePanel.transform);
            go.GetOrAddComponent<ScoreItem>().SetInfo(item.player.publicName, item.rank, item.score);
        }
    }

    void OnClickDelete(PointerEventData data)
    {
        Managers.Sound.Play("Select", Define.Sound.Effect);
        Managers.UI.CloseAllGroupPopupUI(_popupID);
    }

    private void OnDestroy()
    {
        YandexManager.Instance.OnChangeLeaderBoardList -= SetLeaderBoardData;
    }
}

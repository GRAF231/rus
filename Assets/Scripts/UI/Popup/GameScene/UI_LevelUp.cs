using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelUp : UI_Popup
{
    int MaxUpgradeNum = 3;
    
    public enum Panels
    {
        GridPanel,
        
    }

    public override Define.PopupUIGroup _popupID
    {
        get { return Define.PopupUIGroup.UI_LevelUp; }
    }
    public override void Init()
    {
        base.Init();
        Managers.Sound.Play("LevelUp", Define.Sound.Effect);
        Bind<GameObject>(typeof(Panels));

        GameObject gridPanel = Get<GameObject>((int)Panels.GridPanel);

        foreach(Transform child in gridPanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }
        PlayerStat player = Managers.Game.getPlayer().GetOrAddComponent<PlayerStat>();
        List<EventManager.ItemInfo> itemList = Managers.Event.SetRandomItem(player, MaxUpgradeNum);
        for(int i = 0; i<itemList.Count; i++)
        {
            Debug.Log($"Item{i + 1}  : {itemList[i].Name}, {itemList[i].ID}");
        }

        for(int i = 0; i< itemList.Count; i++)
        {
            GameObject upgradePanel = Managers.UI.MakeSubItem<UpgdPanel>(parent:gridPanel.transform).gameObject;
            UpgdPanel upgradeDesc = upgradePanel.GetOrAddComponent<UpgdPanel>();
            upgradeDesc.SetData(itemList[i]);
            var name = itemList[i].Name;
            var title = itemList[i].Title;
            var desc = itemList[i].Desc;
            upgradeDesc.SetInfo(name, title, desc);
        }
    }
}

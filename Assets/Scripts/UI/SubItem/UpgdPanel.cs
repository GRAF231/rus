using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;



public class UpgdPanel : UI_Base
{
    Dictionary<I18NManager.Language, string[]> localizeType = new Dictionary<I18NManager.Language, string[]>(2)
    {
        { I18NManager.Language.en, new string[] { "Weapon", "Skill", "Weapon" } },
        { I18NManager.Language.ru, new string[] { "Оружие", "Навык", "Оружие" } }
    };

    int itemType;
    string itemName;
    enum Texts
    {
        UpgdTitleText,
        UpgdDescText,
        UpgdTypeText
    }

    enum Images
    {
        UpgdInven,
        UpgdImg,
        UpgdPanel,
        UpgdDescPanel,
    }
    public override void Init()
    {

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetImage((int)Images.UpgdPanel).gameObject.AddUIEvent(OnStatOrWeaponUp);
    }

    void OnStatOrWeaponUp(PointerEventData data)
    {
        Managers.Sound.Play("Select", Define.Sound.Effect);
        string title =  Get<TextMeshProUGUI>((int)Texts.UpgdTitleText).text;
        Debug.Log($"{title} select!");
        Managers.Event.LevelUpOverEvent(itemType, itemName);
    }

    public void SetInfo(string name, string title, string desc)
    {
        GetImage((int)Images.UpgdImg).sprite = Managers.Resource.Load<Sprite>($"Prefabs/SpriteIcon/{name}");
        Get<TextMeshProUGUI>((int)Texts.UpgdTitleText).text = title;
        Get<TextMeshProUGUI>((int)Texts.UpgdDescText).text = desc;
    }

    internal void SetData(EventManager.ItemInfo data)
    {
        itemType = data.Type;
        itemName = data.Name;
        Get<TextMeshProUGUI>((int)Texts.UpgdTypeText).text = localizeType[Managers.I18n.Lang][itemType];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_MainMenu : UI_Scene
{
    public Animator _anime;
    public GameObject _container;
    bool _animeOver;


    enum Images
    {
        BackgroundImg,
        FrontImage,
        Logo,
    }

    enum Buttons
    {
        GamePlayButton,
        LangButton,
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        GetImage((int)Images.FrontImage).gameObject.AddUIEvent(SetAnimationOver);

        GetButton((int)Buttons.GamePlayButton).gameObject.AddUIEvent(ShowCharacterSelectUI);
        GetButton((int)Buttons.LangButton).gameObject.AddUIEvent(ChangeLang);
    }

    void SetAnimationOver(PointerEventData data)
    {
        _animeOver = _container.transform.GetComponent<AnimeOver>()._animeOver;
        if (!_animeOver)
        {
            _anime.Play("MainGameStartAnime", -1, 1.0f);
        }
        else
        {
            foreach(Buttons button in System.Enum.GetValues(typeof(Buttons)))
            {
                GetButton((int)button).gameObject.SetActive(true);
            }
            Managers.Resource.Destroy(GetImage((int)Images.FrontImage).gameObject);
        }
    }
    void ShowCharacterSelectUI(PointerEventData data)
    {
        Managers.Sound.Play("Select", Define.Sound.Effect);
        Debug.Log("Show!");
        Managers.UI.ShowPopupUI<UI_CharacterSelect>();
    }

    void ChangeLang(PointerEventData data)
    {
        Managers.I18n.ChangeLang();
    }
}

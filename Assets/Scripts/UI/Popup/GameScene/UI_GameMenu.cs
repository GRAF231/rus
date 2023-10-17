using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameMenu : UI_Popup
{

    enum Buttons
    {
        BackToGameButton,
        BackToMainButton
    }

    enum Images
    {
        BackgroundImage,
    }

    enum Texts
    {
        MenuText,
        SoundText,
    }

    enum Sliders
    {
        VolumeSlider
    }

    Slider volumnSlider;
    TMP_Dropdown BGMdropdown;

    public Image MuteImage;

    public override Define.PopupUIGroup _popupID
    {
        get { return Define.PopupUIGroup.UI_GameMenu; }
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetImage((int)Images.BackgroundImage).gameObject.AddUIEvent(OnBackToGame);
        GetButton((int)Buttons.BackToGameButton).gameObject.AddUIEvent(OnBackToGame);
        GetButton((int)Buttons.BackToMainButton).gameObject.AddUIEvent(OnBackToMain);

        if (Util.isMobile())
        {
            MuteImage.gameObject.SetActive(Managers.Sound.SoundVolume == 0);
        }
        else
        {
            Bind<Slider>(typeof(Sliders));
            volumnSlider = Get<Slider>((int)Sliders.VolumeSlider);
            volumnSlider.value = Managers.Sound.SoundVolume;
        }
    }


    public void OnBackToGame(PointerEventData data)
    {
        Managers.Sound.Play("Select", Define.Sound.Effect);
        Managers.UI.CloseAllGroupPopupUI(Define.PopupUIGroup.UI_GameMenu);
    }
    public void OnBackToMain(PointerEventData data)
    {
        Managers.Sound.Play("Select", Define.Sound.Effect);
        Managers.Scene.LoadScene(Define.SceneType.MainMenuScene);
        Managers.GamePlay();
    }

    public void OnSoundButtonClick()
    {
        Managers.Sound.SoundVolume = Managers.Sound.SoundVolume == 0 ? Managers.Sound.InitVolume : 0;
        MuteImage.gameObject.SetActive(Managers.Sound.SoundVolume == 0);
        Managers.Sound.SetAudioVolumn(Define.Sound.Bgm, Managers.Sound.SoundVolume);
        Managers.Sound.SetAudioVolumn(Define.Sound.Effect, Managers.Sound.SoundVolume);
    }

    public void OnVolumnChanged()
    {
        Managers.Sound.SoundVolume = volumnSlider.value;
        Managers.Sound.SetAudioVolumn(Define.Sound.Bgm, volumnSlider.value);
        Managers.Sound.SetAudioVolumn(Define.Sound.Effect, volumnSlider.value);
    }
}

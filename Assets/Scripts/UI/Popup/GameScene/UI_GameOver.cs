using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_GameOver : UI_Popup
{
    public override Define.PopupUIGroup _popupID { get { return Define.PopupUIGroup.UI_GameOver; } }
    enum Images
    {
        PretectImg,
        DeadImg
    }

    enum Texts
    {
        ScoreText
    }

    enum Buttons
    {
        BackToMainButton
    }

    public override void Init()
    {
        base.Init();
        Managers.Sound.Play("Dead");

        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        var PlayerScore = Managers.Game.getPlayer().GetComponent<PlayerStat>().Score;
        GetText((int)Texts.ScoreText).text = PlayerScore.ToString();
        GetImage((int)Images.PretectImg).gameObject.AddUIEvent(OnClickFinishAnime);
        GetButton((int)Buttons.BackToMainButton).gameObject.AddUIEvent(OnClickBackToMain);

        if (PlayerScore > 1000)
            YandexManager.RateGame();

        YandexManager.SetToLeaderBoard(PlayerScore);
    }

    void OnClickFinishAnime(PointerEventData data)
    {
        GetButton((int)Buttons.BackToMainButton).gameObject.SetActive(true);
    }

    void OnClickBackToMain(PointerEventData data)
    {
        Managers.Sound.Play("Select", Define.Sound.Effect);
        Managers.Scene.LoadScene(Define.SceneType.MainMenuScene);
        Managers.GamePlay();
    }
}

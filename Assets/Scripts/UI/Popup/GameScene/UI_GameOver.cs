using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_GameOver : UI_Popup
{
    public override Define.PopupUIGroup _popupID { get { return Define.PopupUIGroup.UI_GameOver; } }
    public RuntimeAnimatorController[] animeCon;
    Animator anime;
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
        anime = gameObject.FindChild<Animator>("DeadImg");
        anime.runtimeAnimatorController = animeCon[Managers.Game.StartPlayer.id - 1];
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetText((int)Texts.ScoreText).text = Managers.Game.getPlayer().GetComponent<PlayerStat>().Score.ToString();
        GetImage((int)Images.PretectImg).gameObject.AddUIEvent(OnClickFinishAnime);
        GetButton((int)Buttons.BackToMainButton).gameObject.AddUIEvent(OnClickBackToMain);

    }

    void OnClickFinishAnime(PointerEventData data)
    {
        anime.Play("GameOverAnime", -1, 1f);
        GetButton((int)Buttons.BackToMainButton).gameObject.SetActive(true);
    }

    void OnClickBackToMain(PointerEventData data)
    {
        Managers.Sound.Play("Select", Define.Sound.Effect);
        Managers.Scene.LoadScene(Define.SceneType.MainMenuScene);
        Managers.GamePlay();
    }
}

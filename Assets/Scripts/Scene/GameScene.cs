using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameScene : BaseScene
{
    private UI_Player playerUI;

    public override Define.SceneType _sceneType { get { return Define.SceneType.GameScene; } }
    protected override void Init()
    {
        base.Init();
        playerUI = Managers.UI.ShowSceneUI<UI_Player>("UI_Player");
        Managers.ResetGameTime();
        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Player/Player");
        player.GetOrAddComponent<PlayerController>().Init(Managers.Game.StartPlayer);
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
        Managers.Resource.Instantiate("Content/Grid");
    }
    private void Update()
    {
        playerUI.SetPlayerUI();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public static void TogglePauseMenu()
    {
        if (!Managers.gameStop)
        {
            Managers.UI.ShowPopupUI<UI_GameMenu>("UI_GameMenu");
            Managers.GamePause();
        }
        else
        {
            Managers.UI.ClosePopupUI(Define.PopupUIGroup.UI_GameMenu);
        }
    }

    void SetActiveSkillCursorImg()
    {
        if (!Managers.Game.getPlayer().GetOrAddComponent<PlayerStat>().GetWeaponDict().TryGetValue(Define.Weapons.Lightning, out int weapon))
            return;
        Image cursorCoolTimeImg = playerUI.gameObject.FindChild<Image>("CursorCoolTimeImg");
        if (Managers.UI.GetPopupUICount() == 0)
            cursorCoolTimeImg.gameObject.SetActive(true);
        else
            cursorCoolTimeImg.gameObject.SetActive(false);
    }
    public override void Clear()
    {
        Managers.Clear();
    }
}

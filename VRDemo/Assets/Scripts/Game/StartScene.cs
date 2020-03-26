using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : State
{
    private int index;//按键下标
    private UIStartPanel startPanel;

    public StartScene(GameScene scene) : base(scene.ToString())
    {
        
    }

    public override void OnStart()
    {
        GameManage.Register(GameEventType.PlayerSelecting, SelectBtn);//注册玩家正在选择按键
        GameManage.Register(GameEventType.PlayerClickBtn, ClickBtn);//注册点击按键

        UIManager.Instance.ChangePanel(PanelType.UIStart);//打开开始界面
        startPanel = UIManager.Instance.curPanel as UIStartPanel;
    }

    public override void OnEnd()
    {
        GameManage.UnRegister(GameEventType.PlayerSelecting, SelectBtn);//注册玩家正在选择按键
        GameManage.UnRegister(GameEventType.PlayerClickBtn, ClickBtn);//注册点击按键
    }

    private void SelectBtn(object obj)
    {
        PlayerCtrlState state = (PlayerCtrlState)obj;
        switch (state)
        {
            case PlayerCtrlState.Left:
                index--;
                break;
            case PlayerCtrlState.Right:
                index++;
                break;
        }
        index = Mathf.Clamp(index, 0, 1);
        startPanel.UpdateBtnStyle(Color.white, Color.black, Color.black, Color.white,index);
    }

    private void ClickBtn(object obj)
    {
        switch (index)
        {
            case 0:
                //单机
                Game.GoTo(GameScene.Game);
                break;
            case 1:
                //局域网
                Game.GoTo(GameScene.Lobby);
                GameManage.Notify(GameEventType.Start_LanGame, true);
                break;
        }
    }
}


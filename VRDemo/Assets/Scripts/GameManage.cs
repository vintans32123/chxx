using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameEventType
{
    //通用
    PlayerSelecting,//玩家正在左右选择
    PlayerClickBtn,//玩家点击按键
    
    //场景
    Start_LanGame,//局域网
    Lobby_CreatePlayer,//生成大厅玩家
    Lobby_PlayerAllReady,//玩家都已准备好
}

public class GameManage : MonoBehaviour
{
    public static bool playerCanController = true;//玩家能否控制
    public static int mapID;//地图ID

    private static EventManager eventMan = new EventManager();//事件管理器

    private Player player;//玩家

    private void Start()
    {
        player = FindObjectOfType<Player>();
        player.RegisterEvent(PlayerCtrlState.Select, PlayerSelecting);
        player.RegisterEvent(PlayerCtrlState.ClickBtn, PlayerClickBtn);

        DontDestroyOnLoad(gameObject);
    }

    #region 事件处理

    /// <summary>
    /// 玩家选择
    /// </summary>
    /// <param name="obj">玩家选择的方向</param>
    private void PlayerSelecting(object obj)
    {
        PlayerCtrlState ctrlState = (PlayerCtrlState)obj;//获取玩家选择的方向
        Notify(GameEventType.PlayerSelecting, ctrlState);//广播玩家的选择
    }

    /// <summary>
    /// 玩家点击按键
    /// </summary>
    private void PlayerClickBtn(object obj)
    {
        Notify(GameEventType.PlayerClickBtn, true);
    }

    #endregion

    #region 事件

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="fun">需注册的事件</param>
    public static void Register(GameEventType type,EventFun<object> fun)
    {
        eventMan.RegisterEvent((int)type, fun);
    }

    /// <summary>
    /// 解注册事件
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="fun">需解的事件</param>
    public static void UnRegister(GameEventType type,EventFun<object> fun)
    {
        eventMan.UnRegisterEvent((int)type, fun);
    }

    /// <summary>
    /// 广播事件
    /// </summary>
    /// <param name="type">广播类型</param>
    /// <param name="obj">事件参数</param>
    public static void Notify(GameEventType type,object obj)
    {
        eventMan.Notify((int)type, obj);
    }

    #endregion
}

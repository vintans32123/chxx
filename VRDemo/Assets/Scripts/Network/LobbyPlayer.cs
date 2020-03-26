using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// 大厅玩家
/// </summary>
public class LobbyPlayer : NetworkLobbyPlayer
{
    [HideInInspector]
    public PlayerInfo info;//玩家信息
    [SyncVar(hook = "UpdateID")]
    public int mapID;//地图ID

    private LobbyManager lob;//大厅管理器
    private UILobbyPanel uiLobby;//大厅UI
    private Text txt_Name;

    private void Awake()
    {
        uiLobby = UIManager.Instance.curPanel as UILobbyPanel;
    }

    private void Start()
    {
        //初始化
        lob = LobbyManager.singleton as LobbyManager;
        lob.playerCount++;//玩家进入房间则数量 +1

        //玩家信息
        info = new PlayerInfo();

        info.ID = lob.playerCount;
        info.name = "玩家" + info.ID;

        //更新UI
        txt_Name = GetComponentInChildren<Text>();
        txt_Name.text = info.name;

        //广播事件
        GameManage.Notify(GameEventType.Lobby_CreatePlayer, this);

        if (mapID == 0)
        {
            mapID = 1;
        }
        else
        {
            SetParent();
        }
    }

    public override void OnStartLocalPlayer()
    {
        //注册按键点击事件
        GameManage.Register(GameEventType.PlayerClickBtn, PlayerReady);
        GameManage.Register(GameEventType.PlayerSelecting, SelectMap);
    }

    /// <summary>
    /// 选择地图
    /// </summary>
    private void SelectMap(object obj)
    {
        if (readyToBegin) return;

        if (!isLocalPlayer) return;

        //获得玩家方向
        PlayerCtrlState ctrlState = (PlayerCtrlState)obj;
        CmdSelect(ctrlState);
    }

    [Command]
    private void CmdSelect(PlayerCtrlState ctrlState)
    {
        RpcSelect(ctrlState);
    }

    [ClientRpc]
    private void RpcSelect(PlayerCtrlState ctrlState)
    {
        //得到玩家方向
        switch (ctrlState)
        {
            case PlayerCtrlState.Left:
                mapID--;
                break;
            case PlayerCtrlState.Right:
                mapID++;
                break;
        }
    }

    /// <summary>
    /// 更新 mapID （当 mapID 的值发生修改时，设置玩家父级）
    /// </summary>
    /// <param name="id"></param>
    private void UpdateID(int id)
    {
        mapID = id;
        mapID = Mathf.Clamp(mapID, 1, uiLobby.maxMapNum);
        SetParent();
    }

    /// <summary>
    /// 设置父级
    /// </summary>
    private void SetParent()
    {
        if (mapID <= 0 || mapID > uiLobby.maxMapNum) return;

        Transform tmpParent = uiLobby.GetLobbyPlayerParent(mapID);//获得父级
        if (tmpParent != null && transform.parent != tmpParent)
        {
            transform.SetParent(tmpParent);
        }
    }

    /// <summary>
    /// 当玩家准备
    /// </summary>
    private void PlayerReady(object obj)
    {
        CmdReady();
    }

    [Command]
    private void CmdReady()
    {
        RpcReady();
    }

    [ClientRpc]
    private void RpcReady()
    {
        if (!readyToBegin)
        {
            readyToBegin = true;

            //更新 UI 显示
            txt_Name.text += "准备";

            //检测其他玩家是否准备
            lob.CheckReady();
        }
    }
}

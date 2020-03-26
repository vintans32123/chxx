using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LobbyScene : State
{
    private List<LobbyPlayer> playerList;

    //UI
    private UILobbyPanel lobbyPanel;//大厅界面

    public LobbyScene(GameScene scene) : base(scene.ToString())
    {
        playerList = new List<LobbyPlayer>();
    }

    public override void OnStart()
    {
        //注册事件
        GameManage.Register(GameEventType.Lobby_CreatePlayer, CreatePlayer);//注册生成大厅玩家
        GameManage.Register(GameEventType.Lobby_PlayerAllReady, PlayerAllReady);//注册确认选择地图

        UIManager.Instance.ChangePanel(PanelType.UILobby);
        lobbyPanel = UIManager.Instance.curPanel as UILobbyPanel;
    }

    public override void OnEnd()
    {
        //解注册事件
        GameManage.UnRegister(GameEventType.Lobby_CreatePlayer, CreatePlayer);
        GameManage.UnRegister(GameEventType.Lobby_PlayerAllReady, PlayerAllReady);
    }

    /// <summary>
    /// 玩家都已经准备好
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerAllReady(object obj)
    {
        if (playerList.Count > 0)
        {
            if (playerList.Count <= 1)//如果只有 1 个玩家
            {
                Debug.Log("只有一个玩家在线，选择了地图：" + playerList[0].mapID);
            }
            else
            {
                //进行筛选，选择最多的地图优先使用
                FilterMap();
            }
        }
        Game.GoTo(GameScene.Game);//去游戏场景
    }

    /// <summary>
    /// 筛选地图
    /// </summary>
    private void FilterMap()
    {
        Dictionary<int, int> tmpDic = new Dictionary<int, int>();
        for (int i = 1; i < lobbyPanel.maxMapNum + 1; i++)
        {
            int count = 0;
            for (int j = 0; j < playerList.Count; j++)
            {
                if (i == playerList[j].mapID)//判断某张地图被几个玩家同时选中
                {
                    count++;
                }
            }
            tmpDic.Add(i, count);//当某张地图所有玩家都判断完之后，把选择的地图和次数存入字典中
        }

        //字典排序(倒序)
        var dicSort = from objDic in tmpDic orderby objDic.Value descending select objDic;
        KeyValuePair<Int32, Int32> pair = dicSort.First();//取得排序后的第一个键值对

        string format = string.Format("地图: {0} 有 {1} 个玩家选择.", pair.Key, pair.Value);
        Debug.Log(format);
    }

    /// <summary>
    /// 生成大厅玩家时添加进集合
    /// </summary>
    /// <param name="obj">生成的玩家</param>
    private void CreatePlayer(object obj)
    {
        LobbyPlayer player = obj as LobbyPlayer;
        if (!playerList.Contains(player))
        {
            playerList.Add(player);
        }
    }
}

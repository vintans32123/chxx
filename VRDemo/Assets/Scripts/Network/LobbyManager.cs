using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager
{
    public LobbyDiscover discover;//网络搜索器
    public static bool isFind;//是否找到
    public int playerCount;//玩家数量

    private LobbyHook lobbyHook;

    private void Start()
    {
        GameManage.Register(GameEventType.Start_LanGame, LanGame);//注册加入局域网事件

        lobbyHook = GetComponent<LobbyHook>();
    }

    /// <summary>
    /// 局域网游戏
    /// </summary>
    private void LanGame(object obj)
    {
        singleton.StartCoroutine((singleton as LobbyManager).DiscoverConnect());
    }

    #region 大厅

    private IEnumerator DiscoverConnect()
    {
        discover.Initialize();//初始化
        discover.StartAsClient();//尝试连接，如果有主机，则直接连接

        yield return new WaitForSeconds(1f);

        //如果没有搜索到则自己创建主机
        if (!isFind)
        {
            discover.StopBroadcast();//停止广播

            yield return new WaitForSeconds(0.5f);

            StartHost();//创建主机

            discover.Initialize();//初始化
            discover.StartAsServer();//作为服务器进行广播
        }
    }

    /// <summary>
    /// 当有大厅玩家进入大厅时
    /// </summary>
    public override void OnLobbyClientEnter()
    {
        UIManager.Instance.ChangePanel(PanelType.UILobby);//打开大厅界面
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject obj1 = Resources.Load<GameObject>("Prefabs/E37");
        GameObject obj2 = Instantiate<GameObject>(obj1);

        return obj2;
    }

    /// <summary>
    /// 检查玩家是否准备
    /// </summary>
    public void CheckReady()
    {
        if (GetReady())
        {
            GameManage.Notify(GameEventType.Lobby_PlayerAllReady, true);   //广播游戏开始 

            ServerChangeScene(playScene);//跳场景
        }
    }

    /// <summary>
    /// 检测玩家是否都准备好
    /// </summary>
    private bool GetReady()
    {
        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            if (lobbySlots[i] != null && !lobbySlots[i].readyToBegin)
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    #region 游戏

    /// <summary>
    /// 当客户端切换场景时
    /// </summary>
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        UIManager.Instance.ChangePanel(PanelType.None);//当切换场景时，关闭当前界面
        base.OnClientSceneChanged(conn);
    }

    /// <summary>
    /// 当大厅服务器加载玩家时
    /// </summary>
    /// <param name="lobbyPlayer">大厅玩家</param>
    /// <param name="gamePlayer">游戏玩家</param>
    /// <returns></returns>
    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        if (lobbyHook != null)
        {
            lobbyHook.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);
        }
        return true;
    }

    /// <summary>
    /// 获取出生点位置
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector3 GetSpawnPos(int index)
    {
        return startPositions[index].position;
    }

    #endregion
}


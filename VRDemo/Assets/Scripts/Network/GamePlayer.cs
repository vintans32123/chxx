using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar(hook = "UpdateInfo")]
    public PlayerInfo info;//玩家信息

    private void Start()
    {
        LobbyManager lob = LobbyManager.singleton as LobbyManager;

        //设置初始位置
        transform.position = lob.GetSpawnPos(info.ID - 1);//获取出生点位置

        //Text txt_Name = transform.Find("Canvas/txt_Name").GetComponent<Text>();
        //txt_Name.text = info.name;

        gameObject.name = "Player_" + info.ID;

        Debug.Log("GamePlayerStart");
    }

    public override void OnStartLocalPlayer()
    {
        //设置本地玩家初始化信息及显示
        PlayerController playerCon = gameObject.AddComponent<PlayerController>();
        playerCon.InitCar(transform, this);

        Debug.Log("OnStartLocalPlayer");
    }

    /// <summary>
    /// 更新玩家信息
    /// </summary>
    /// <param name="varInfo">玩家信息</param>
    private void UpdateInfo(PlayerInfo varInfo)
    {
        info = varInfo;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 玩家信息
/// </summary>
public struct PlayerInfo
{
    public string name;//玩家姓名
    public int ID;//玩家 ID
    public int selectMapID;//选择的地图ID
}

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager man, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        GamePlayer game = gamePlayer.GetComponent<GamePlayer>();

        game.info = lobby.info;
    }
}

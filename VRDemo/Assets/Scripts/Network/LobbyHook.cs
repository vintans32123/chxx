using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class LobbyHook : MonoBehaviour
{
    public virtual void OnLobbyServerSceneLoadedForPlayer(NetworkManager man, GameObject lobbyPlayer, GameObject gamePlayer)
    {

    }
}

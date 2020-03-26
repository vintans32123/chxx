using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyDiscover : NetworkDiscovery
{
    /// <summary>
    /// 当接受到广播信息后执行的回调
    /// </summary>
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        LobbyManager.isFind = true;

        //获取服务器 IP 地址，进行连接
        LobbyManager.singleton.networkAddress = fromAddress;
        LobbyManager.singleton.StartClient();

        Invoke("Check", 0.5f);
    }

    private void Check()
    {
        StopBroadcast();
    }
}

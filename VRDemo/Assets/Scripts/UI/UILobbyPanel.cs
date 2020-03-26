using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILobbyPanel : UIBasePanel
{
    [HideInInspector]
    public int maxMapNum;//地图最大数量

    private void Awake()
    {
        maxMapNum = transform.childCount - 1;
    }

    /// <summary>
    /// 获取大厅玩家父级
    /// </summary>
    /// <param name="index">父级下标</param>
    public Transform GetLobbyPlayerParent(int index)
    {
        return transform.GetChild(index).Find("playerGrid");
    }
}

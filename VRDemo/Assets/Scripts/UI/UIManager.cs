using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType
{
    None = -1,//不打开界面
    UIStart = 0,//开始界面
    UILobby,//大厅界面
    UIGame,//游戏界面
    UIOver//结束界面
}

/// <summary>
/// UI管理器
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
    }

    public UIBasePanel curPanel;//当前界面

    private List<UIBasePanel> allPanel;//所有界面
    private void Awake()
    {
        _instance = this;

        DontDestroyOnLoad(gameObject);

        allPanel = new List<UIBasePanel>();

        //初始化
        foreach (Transform item in transform)
        {
            UIBasePanel basePanel = item.GetComponent<UIBasePanel>();
            allPanel.Add(basePanel);//添加所有界面
        }
        curPanel = allPanel[(int)PanelType.UIStart];//初始化当前界面
    }

    /// <summary>
    /// 更换界面
    /// </summary>
    /// <param name="openPanel">需要打开的界面</param>
    public void ChangePanel(PanelType openPanel)
    {
        //关闭界面
        if (curPanel.gameObject.activeSelf)
        {
            curPanel.gameObject.SetActive(false);
        }

        if (openPanel != PanelType.None)//如果有需要打开的界面才打开
        {
            //打开界面
            curPanel = allPanel[(int)openPanel];
            if (!curPanel.gameObject.activeSelf)
            {
                curPanel.gameObject.SetActive(true);
            }
        }
    }
}

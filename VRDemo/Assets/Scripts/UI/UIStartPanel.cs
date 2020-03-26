using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏开始界面
/// </summary>
public class UIStartPanel : UIBasePanel
{
    private Transform frame;//选框
    private List<Image> allBtn;//所有的按键
    private Image curBtn;//当前选中的按键

    private void Start()
    {
        allBtn = new List<Image>();
        allBtn.Add(transform.Find("btn_SinglePlayer").GetComponent<Image>());
        allBtn.Add(transform.Find("btn_Multiplayer").GetComponent<Image>());

        frame = transform.Find("frame");

        curBtn = allBtn[0];//当前为第一个按键
    }

    /// <summary>
    /// 更新按键样式
    /// </summary>
    /// <param name="oldImg">之前选中的图片</param>
    /// <param name="oldTxt">之前显示的文本</param>
    /// <param name="newImg">当前选中的图片</param>
    /// <param name="newTxt">当前显示的文本</param>
    public void UpdateBtnStyle(Color oldImg,Color oldTxt,Color newImg,Color newTxt,int index)
    {
        //更换之前选中的按键的样式
        curBtn.color = oldImg;
        Text show1 = curBtn.transform.GetComponentInChildren<Text>();
        show1.color = oldTxt;

        //更新当前选中的按键
        curBtn = allBtn[index];
        curBtn.color = newImg;
        Text show2 = curBtn.transform.GetComponentInChildren<Text>();
        show2.color = newTxt;

        //更新选框位置
        frame.position = curBtn.transform.position;
    }
}

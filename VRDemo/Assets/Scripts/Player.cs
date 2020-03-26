using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家的操控状态
/// </summary>
public enum PlayerCtrlState
{
    None,
    Left,
    Right,
    ClickBtn,
    Select
}

/// <summary>
/// 玩家
/// </summary>
public class Player : MonoBehaviour
{
    private float wheelOffset;//方向盘偏移量
    private WheelInput wheelInput;//方向盘输入
    private EventManager eventMan = new EventManager();//事件管理器

    private bool isTriggerClick;//触发点击
    private float timer;//广播计时器

    private void Start()
    {
        wheelInput = WheelInput.Instance;
        wheelOffset = wheelInput.steeringWheel;

        DontDestroyOnLoad(gameObject);
    }

    #region 事件

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="state">事件ID</param>
    /// <param name="fun">注册的事件</param>
    public void RegisterEvent(PlayerCtrlState state,EventFun<object> fun)
    {
        eventMan.RegisterEvent((int)state, fun);
    }

    /// <summary>
    /// 解注册
    /// </summary>
    /// <param name="state">事件ID</param>
    /// <param name="fun">需解注册的事件</param>
    public void UnRegisterEvent(PlayerCtrlState state,EventFun<object> fun)
    {
        eventMan.UnRegisterEvent((int)state, fun);
    }

    /// <summary>
    /// 广播事件
    /// </summary>
    /// <param name="state">事件ID</param>
    /// <param name="obj">事件参数</param>
    public void NotifyEvent(PlayerCtrlState state,object obj)
    {
        eventMan.Notify((int)state, obj);
    }

    #endregion

    private void Update()
    {
        if (!GameManage.playerCanController) return;//当玩家不允许控制时

        wheelInput.InputValue();

        timer += Time.deltaTime;
        if (timer >= 0.5f)//每隔 0.5 秒广播一次
        {
            //执行方向盘转向操作
            NotifyEvent(PlayerCtrlState.Select, IsRight());//广播事件

            timer = 0;
        }

        //确认点击
        if (EnterClick())
        {
            if (isTriggerClick)
            {
                NotifyEvent(PlayerCtrlState.ClickBtn, true);
                isTriggerClick = false;
            }
        }
    }

    /// <summary>
    /// 确实是否点击确定
    /// </summary>
    private bool EnterClick()
    {
        //当抬起脚时，打开按键开关
        if (wheelInput.accelerator <= 0.5f && !isTriggerClick)
        {
            isTriggerClick = true;
        }
        return wheelInput.accelerator > 0.5f;
    }

    /// <summary>
    /// 获取方向盘方向
    /// </summary>
    private PlayerCtrlState IsRight()
    {
        if (wheelInput.steeringWheel > 0.1f)
        {
            return PlayerCtrlState.Right;
        }
        else if(wheelInput.steeringWheel < -0.1f)
        {
            return PlayerCtrlState.Left;
        }
        else
        {
            return PlayerCtrlState.None;
        }
    }
}

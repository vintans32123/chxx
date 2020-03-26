using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 方向盘输入
/// </summary>
public class WheelInput : MonoBehaviour
{
    private static WheelInput _instance;
    public static WheelInput Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        LogitechGSDK.LogiSteeringInitialize(false);//外设初始化

        DontDestroyOnLoad(gameObject);//加载不销毁
    }

    public float steeringWheel;//方向盘
    public float accelerator;//油门
    public float footbrake;//脚刹
    public float clutch;//离合器
    public bool isStartEngine;//是否启动引擎
    public bool isOpenLowHeadLight;//是否打开近光灯
    public bool isOpenHighHeadLight;//是否打开远光灯
    public bool isOpenRightIndicator;//是否打开右指示灯
    public bool isOpenLeftIndicator;//是否打开左指示灯
    public bool isOpenDoubleFlash;//是否打开双闪等
    public bool isUseHandbrake;//是否使用手刹

    private void Update()
    {
        InputValue();
    }

    /// <summary>
    /// 输入方式
    /// </summary>
    public void InputValue()
    {
        //判断设备是否接入
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            //获取键位值
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            steeringWheel = rec.lX / 32767f;//获取方向盘偏移量
            accelerator = Mathf.Clamp01(rec.lY / -32767f);//获取油门偏移量
            footbrake = Mathf.Clamp01(rec.lRz / -32767f);//获取刹车偏移量
            clutch = rec.rglSlider[0] / -32767f;//获取偏移量
            isStartEngine = LogitechGSDK.LogiButtonTriggered(0, 23);//判断是否启动引擎23
            isOpenLowHeadLight = LogitechGSDK.LogiButtonTriggered(0, 11);//判断是否打开近光灯11
            isOpenHighHeadLight = LogitechGSDK.LogiButtonTriggered(0, 10);//判断是否打开远光灯10
            isOpenRightIndicator = LogitechGSDK.LogiButtonTriggered(0, 6);//判断是否打开右指示灯6
            isOpenLeftIndicator = LogitechGSDK.LogiButtonTriggered(0, 7);//判断是否打开左指示灯7
            isUseHandbrake = LogitechGSDK.LogiButtonTriggered(0, 2);//判断是否使用手刹2
            isOpenDoubleFlash = LogitechGSDK.LogiButtonTriggered(0, 5);//判断是否打开双闪灯5
        }
        else
        {
            steeringWheel = Input.GetAxis("Horizontal");//获取方向盘偏移量
            accelerator = Mathf.Clamp01(Input.GetAxis("Vertical"));//获取油门偏移量
            footbrake = Mathf.Clamp01(-Input.GetAxis("Vertical"));//获取刹车偏移量
        }
    }
}

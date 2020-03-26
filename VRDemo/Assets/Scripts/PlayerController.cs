using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试玩家
/// </summary>
public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public GameObject carCamera;//摄像机

    public void InitCar(Transform varParent,GamePlayer player)
    {
        if (player.isLocalPlayer)
        {
            carCamera = GameObject.FindObjectOfType<RCC_Camera>().gameObject;
            carCamera.SetActive(false);
        }

        //RCC_CarControllerV3 carPrefab = Resources.Load<RCC_CarControllerV3>("Prefabs/E37");//加载预制体
        //RCC_CarControllerV3 car = Instantiate<RCC_CarControllerV3>(carPrefab);//实例化车
        //car.transform.SetParent(varParent);//设置车辆父级
        //car.transform.localPosition = Vector3.zero;
        //car.transform.localRotation = Quaternion.identity;
        //car.transform.localScale = Vector3.one;

        if (player.isLocalPlayer)
        {
            RCC_EnterExitCar rccCar = gameObject.AddComponent<RCC_EnterExitCar>();
            rccCar.SetCarCamera(carCamera);
        }

        GameObject ob = new GameObject("Player");

        Transform playerObj = transform.Find("PlayerObj");
        gameObject.SendMessage("Act", ob, SendMessageOptions.DontRequireReceiver);
        GetComponent<RCC_CarControllerV3>().SetPlayer(player);

        Debug.Log("PlayerControllerInitCar");
    }
}

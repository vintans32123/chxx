using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerRotation : NetworkBehaviour
{
    public enum RotaType
    {
        Head,
        Hand,
    }
    
    public RotaType curType = RotaType.Head;
    public Transform upperBody;//上半身

    private Transform eye;//眼睛
    private Transform rightHand;//右手
    private bool isRota;

    [SyncVar]
    private bool isBodyRota;
    [SyncVar]
    private Quaternion allBodyRota;
    [SyncVar]
    private Quaternion upperBodyRota;

    private void LateUpdate()
    {
        if (isLocalPlayer)
        {
            //身体
            BodyRotation();

            //通知服务器更新数据
            CmdSendServerPos(transform.rotation, upperBody.rotation,isRota);
        }
        else
        {
            LerpRota();
        }
    }

    [Command]
    private void CmdSendServerPos(Quaternion varBodyRota, Quaternion varUpperBodyEuler,bool varRota)
    {
        isBodyRota = varRota;

        allBodyRota = varBodyRota;
        if (isBodyRota)
        {
            upperBodyRota = varUpperBodyEuler;
        }
    }

    private void LerpRota()
    {
        if (isBodyRota)
        {
            upperBody.rotation = upperBodyRota;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, allBodyRota, 5 * Time.fixedDeltaTime);
    }

    //身体旋转
    private void BodyRotation()
    {
        if (curType == RotaType.Head)
        {
            Vector3 targetEuler = new Vector3(transform.localEulerAngles.x, eye.localEulerAngles.y, transform.localEulerAngles.z);
            //旋转
            transform.localEulerAngles = targetEuler;
            isRota = false;
        }
        else
        {
            Vector3 targetPoint = new Vector3(rightHand.position.x,transform.position.y,rightHand.position.z);//手的相对位置
            Vector3 handDir = (targetPoint - transform.position).normalized;//手的方向
            float dis = Vector3.Distance(eye.position, rightHand.position);//得到头盔和右手的距离

            if (dis > 0.6 && rightHand.position.y <= 1f)
            {
                //自身随着头盔旋转
                Vector3 targetEuler = new Vector3(transform.localEulerAngles.x, eye.localEulerAngles.y, transform.localEulerAngles.z);
                transform.localEulerAngles = targetEuler;
                isRota = false;
            }
            else
            {
                transform.forward = handDir;
                isRota = true;
            }
        }
    }
}

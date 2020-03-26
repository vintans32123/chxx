using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
    public float moveOffset = 0.003f;

    [SyncVar]
    private Vector3 pos;

    private Transform eye;//眼睛

    private void Update()
    {
        if (!isLocalPlayer) return;

        Move();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            CmdSendServerPos(transform.position);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, pos, 5 * Time.fixedDeltaTime);
        }
    }

    [Command]
    private void CmdSendServerPos(Vector3 varPos)
    {
        pos = varPos;
    }

    private void Move()
    {
        //transform.position = v;
    }
}

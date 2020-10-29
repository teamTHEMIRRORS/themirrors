using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove_sagyou : MonoBehaviour
{
    public float speed = 5;
    public Vector2 direction;
    bool pushright = false;
    bool pushleft = false;

    //ボタンの検知
    public void PushDownRight()
    {
        //PhotonView photonView;
        //photonView = GetComponent<PhotonView>();
        //if (photonView.IsMine)
        //{
            pushright = true;
        //}
        //else
        //{
            //Debug.Log("IsMine = false");
        //}

    }

    public void PushDownLeft()
    {
        //PhotonView photonView;
        //photonView = GetComponent<PhotonView>();
        //if (photonView.IsMine)
        //{
            pushleft = true;
        //}
        //else
        //{
            //Debug.Log("IsMine = false");
        //}

    }

    public void PushUp()
    {
        //PhotonView photonView;
        //photonView = GetComponent<PhotonView>();
        //if (photonView.IsMine)
        //{
            pushright = false;
            pushleft = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        //}
        //else
        //{
            //Debug.Log("IsMine = false");
        //}

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            PushDownLeft();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            PushDownRight();
        }

        if (Input.GetKeyUp(KeyCode.A) | Input.GetKeyUp(KeyCode.S))
        {
            PushUp();
        }
        */
        if (pushright == true | pushleft == true)
        {
            Move();
        }

    }

    //プレイヤーの移動
    public void Move()
    {
        if (pushright)
        {
            direction = new Vector2(1.0f, 0).normalized;
        }
        else
        {
            direction = new Vector2(-1.0f, 0).normalized;
        }

        GetComponent<Rigidbody2D>().velocity = direction * speed;

    }
}

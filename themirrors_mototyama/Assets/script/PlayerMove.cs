using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPun
{
    public float speed = 5;
    public Vector2 direction;
    bool pushright = false;
    bool pushleft = false;
    SpriteRenderer playersprite;
    Animator anim;
    string flipflag;
    int flipcount = 0;


    //ボタンの検知
    public void PushDownRight()
    {
        PhotonView photonView;
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            pushright = true;
        }
        else
        {
            Debug.Log("IsMine = false");
        }
        
    }

    public void PushDownLeft()
    {
        PhotonView photonView;
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            pushleft = true;
        }
        else
        {
            Debug.Log("IsMine = false");
        }
        
    }

    public void PushUp()
    {
        PhotonView photonView;
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            pushright = false;
            pushleft = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        }
        else
        {
            Debug.Log("IsMine = false");
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        
        
        if (pushright == true | pushleft == true)
        {
            //speed = 5f;
            anim.SetBool("walk", true);
            Move();
        }
        else
        {
            //playersprite.flipX = false;
            anim.SetBool("walk", false);
        }


        
    }

    private void Awake()
    {
        playersprite = this.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    //プレイヤーの移動
    public void Move()
    {


        if (pushright)
        {
            //this.gameObject.transform.localScale = new Vector3(-1,1,1);
            flipflag = "right";
            if(flipcount == 0)
            {
                photonView.RPC("Flipplayer", RpcTarget.All, flipflag);
                flipcount = 1; 
            }
            //photonView.RPC("Flipplayer", RpcTarget.All, flipflag);
            direction = new Vector2(1.0f, 0).normalized;
        }
        else
        {
            flipflag = "left";
            //this.gameObject.transform.localScale = new Vector3(1,1,1);
            if(flipcount == 1)
            {
                photonView.RPC("Flipplayer", RpcTarget.All, flipflag);
                flipcount = 0;
            }
            //photonView.RPC("Flipplayer", RpcTarget.All, flipflag);
            direction = new Vector2(-1.0f, 0).normalized;
        }

        GetComponent<Rigidbody2D>().velocity = direction * speed;

    }

    [PunRPC]
    void Flipplayer(string flipflag)
    {
        if(flipflag == "right")
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(flipflag == "left")
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

}

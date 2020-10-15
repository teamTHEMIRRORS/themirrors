using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5;
    public Vector2 direction;
    bool pushright = false;
    bool pushleft = false;

    //ボタンの検知
    public void PushDownRight()
    {
        pushright = true;
    }

    public void PushDownLeft()
    {
        pushleft = true;
    }

    public void PushUp()
    {
        pushright = false;
        pushleft = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f,0.0f);
    }
    
    // Update is called once per frame
    void Update()
    {
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

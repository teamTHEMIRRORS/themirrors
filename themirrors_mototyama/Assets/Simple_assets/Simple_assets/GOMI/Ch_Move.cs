using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch_Move : MonoBehaviour
{
    //変数定義
    [SerializeField]
    private float m_speed = 1.0f;
    [SerializeField]
    private Vector2 m_moveDirection = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //キーボード操作
        Vector3 move = m_moveDirection * m_speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += move;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= move;
        }   
    }

    private void OnCollisionStay2D(Collision2D i_collision)
    {
        var normal = i_collision.contacts[0].normal;

        Vector2 dir = m_moveDirection - Vector2.Dot(m_moveDirection, normal) * normal;
        m_moveDirection = dir.normalized;
    }
}

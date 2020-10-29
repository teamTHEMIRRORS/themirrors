using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks
{
    public static GameObject bottunList;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            UIManager.bottunList = this.gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

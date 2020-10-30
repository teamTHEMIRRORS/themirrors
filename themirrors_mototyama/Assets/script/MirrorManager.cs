using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MirrorManager : MonoBehaviourPun
{
    GameObject gamemanager;
    GameManager gamemanagerscript;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        gamemanager = GameObject.Find("GameManager");
        gamemanagerscript = gamemanager.GetComponent<GameManager>();
    }

    public void Breaking()
    {
        photonView.RPC("BreakMirror",RpcTarget.All);
    }

    [PunRPC]
    void BreakMirror()
    {
        gamemanagerscript.breakedmirror += 1;
        Destroy(this.gameObject);
    }



}

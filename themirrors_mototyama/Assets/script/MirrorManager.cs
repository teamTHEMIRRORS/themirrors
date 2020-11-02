using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MirrorManager : MonoBehaviourPun
{
    GameObject gamemanager;
    GameManager gamemanagerscript;
    public GameObject mirror_intheotherworld;
    public GameObject warppointer;
    public int breaklevel = 0;
    public Sprite mirror_break1;
    public Sprite mirror_break2;
    public Sprite mirror_break3;
    public Sprite mirror_break4;
    

    private void Awake()
    {
        gamemanager = GameObject.Find("GameManager");
        gamemanagerscript = gamemanager.GetComponent<GameManager>();
        
}

    public void Breaking()
    {
        breaklevel++;
        photonView.RPC("BreakMirror",RpcTarget.All,breaklevel);
        //gamemanagerscript.Clear();
    }

    [PunRPC]
    void BreakMirror(int breaklevel)
    {
        
        if(breaklevel == 1)
        {
            GetComponent<SpriteRenderer>().sprite = mirror_break1;
            mirror_intheotherworld.GetComponent<SpriteRenderer>().sprite = mirror_break1;
        }
        else if (breaklevel == 2)
        {
            GetComponent<SpriteRenderer>().sprite = mirror_break2;
            mirror_intheotherworld.GetComponent<SpriteRenderer>().sprite = mirror_break2;
        }
        else if (breaklevel == 3)
        {
            GetComponent<SpriteRenderer>().sprite = mirror_break3;
            mirror_intheotherworld.GetComponent<SpriteRenderer>().sprite = mirror_break3;
        }
        else if (breaklevel == 4)
        {
            GetComponent<SpriteRenderer>().sprite = mirror_break4;
            mirror_intheotherworld.GetComponent<SpriteRenderer>().sprite = mirror_break4;
        }
        else if(breaklevel == 5)
        {
            Destroy(this.gameObject);
            Destroy(mirror_intheotherworld);
            gamemanagerscript.breakedmirror += 1;
            gamemanagerscript.Clear();
        }
        
    }
    
}

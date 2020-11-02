using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class PlayerManager : MonoBehaviourPunCallbacks//,IPunObservable
{
    #region Public Field

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public int HP = 100;
    public bool breakmirror = false;
    public bool upstairs = false;
    public bool downstairs = false;
    public bool dontMove = false;
    public GameObject gamemanager;
    GameManager gamemanagerscript;
    GameObject mirror;
    GameObject stairs;
    MirrorManager mirrormanager;
    BloodEffectObject bloodeffectobject;
    UpStairs up_stair;
    DownStairs down_stair;
    Animator playeranim;
    

    float time = 0.0f;

    #endregion

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }

        //DontDestroyOnLoad(this.gameObject);
        gamemanager = GameObject.Find("GameManager");
        gamemanagerscript = gamemanager.GetComponent<GameManager>();
        bloodeffectobject = this.GetComponentInChildren<BloodEffectObject>();
        playeranim = GetComponent<Animator>();

    }

    

    private void Update()
    {
        if(HP <= 0 && photonView.IsMine)
        {
            bloodeffectobject.damage = 1.0f;

            time += Time.deltaTime;
            dontMove = true;
            if(time > 1.5f)
            {
                gamemanagerscript.LeaveRoom();
                SceneManager.LoadScene("surv_lose");
                dontMove = false;
            }
            
        }

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mirror"))
        {
            //Debug.Log("hit mirror");
            breakmirror = true;
            mirror = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("UpStairs"))
        {
            upstairs = true;
            stairs = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("DownStairs"))
        {
            downstairs = true;
            stairs = collision.gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mirror"))
        {
            breakmirror = false;
        }
        else if (collision.gameObject.CompareTag("UpStairs"))
        {
            upstairs = false;
        }
        else if (collision.gameObject.CompareTag("DownStairs"))
        {
            downstairs = false;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            GetComponent<BoxCollider2D>().isTrigger = true;

        }
    }

    #region

#if !UNITY_5_4_OR_NEWER
    /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }
#endif


    //void CalledOnLevelWasLoaded(int level)
    //{
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        //if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        //{
            //transform.position = new Vector3(0f, 0f, 0f);
        //}
    //}
    #endregion

    
    public void SurviorAction()
    {
        if (breakmirror)
        {
            
            mirrormanager = mirror.GetComponent<MirrorManager>();
            StartCoroutine("Breakcoroutine");
            mirrormanager.Breaking();
        }
        else if (upstairs)
        {
            up_stair = stairs.GetComponent<UpStairs>();
            this.gameObject.transform.position = up_stair.stairs_up.transform.position;
        }
        else if (downstairs)
        {
            down_stair = stairs.GetComponent<DownStairs>();
            this.gameObject.transform.position = down_stair.stairs_down.transform.position;
        }

        //breakmirror = false;
        upstairs = false;
        downstairs = false;
    }

    

    public void Damageplayer()
    {
        photonView.RPC("KilledPlayer", RpcTarget.All, this.HP);
    }

    [PunRPC]
    void KilledPlayer(int HP)
    {
        gamemanagerscript.killedplayer += 1;
        this.HP -= 100;
    }


    private IEnumerator Breakcoroutine()
    {
        playeranim.SetBool("break", true);
        //dontMove = true;
        GetComponent<PlayerMove>().speed = 0;
        yield return new WaitForSeconds(1.0f);
        playeranim.SetBool("break", false);
        //dontMove = false;
        GetComponent<PlayerMove>().speed = 7;
    }
}

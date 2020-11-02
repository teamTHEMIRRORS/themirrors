using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class KillerManager : MonoBehaviourPunCallbacks
{
    #region Public Field

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalKillerInstance;

    public bool kill = false;
    public bool dontmove = false;
    public bool entermirror = false;
    public bool upstairs = false;
    public bool downstairs = false;
    public bool in_mirrorworld = false;
    public bool exitmirrorworld = false;
    UpStairs up_stair;
    DownStairs down_stair;
    MirrorManager mirrorManager;
    GameObject stairs;
    GameObject player;
    GameObject mirror;
    GameObject gamemanager;
    GameManager gamemanagerscript;
    PlayerManager playerManager;

    #endregion

    private void Awake()
    {
        if (photonView.IsMine)
        {
            KillerManager.LocalKillerInstance = this.gameObject;
        }

        gamemanager = GameObject.Find("GameManager");
        gamemanagerscript = gamemanager.GetComponent<GameManager>();
        //DontDestroyOnLoad(this.gameObject);

    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("hitplayer");
            kill = true;
            player = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("Mirror") || collision.gameObject.CompareTag("windows"))
        {
            //Debug.Log("hitMirror");
            mirror = collision.gameObject;
            entermirror = true;
        }
        else if (collision.gameObject.CompareTag("Mirror_inthemirrorworld") || collision.gameObject.CompareTag("windows_inthemirrorworld"))
        {
            //Debug.Log("hit other world Mirror");
            mirror = collision.gameObject;
            exitmirrorworld = true;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("you can't kill player");
            kill = false;

        }
        else if (collision.gameObject.CompareTag("Mirror") || collision.gameObject.CompareTag("windows"))
        {
            //Debug.Log("you can't enter mirror");
            entermirror = false;
        }
        else if (collision.gameObject.CompareTag("Mirror_inthemirrorworld") || collision.gameObject.CompareTag("windows_inthemirrorworld"))
        {
            exitmirrorworld = false;
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

    private void Start()
    {
        #if UNITY_5_4_OR_NEWER
        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        };
        #endif
    }

    #region

    #if !UNITY_5_4_OR_NEWER
    /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }
    #endif


    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 0f, 0f);
        }
    }
    #endregion

    public void KillerAction()
    {
        if (kill)
        {
            //Animator killanim = GetComponent<Animator>();
            //killanim.SetBool("kill", true);
            playerManager = player.GetComponent<PlayerManager>();
            StartCoroutine("AnimCoroutine");
            //playerManager.HP -= 1000;
            playerManager.Damageplayer();
            
        }
        else if (entermirror)
        {
            mirrorManager = mirror.GetComponent<MirrorManager>();
            in_mirrorworld = true;
            GetComponent<PlayerMove>().speed = 21;
            this.gameObject.transform.position = mirrorManager.warppointer.transform.position;
            GetComponent<KillerEffectObject>().InnerMirror = true;
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
        else if (exitmirrorworld)
        {
            mirrorManager = mirror.GetComponent<MirrorManager>();
            in_mirrorworld = false; ;
            GetComponent<PlayerMove>().speed = 7;
            this.gameObject.transform.position = mirrorManager.warppointer.transform.position;
            GetComponent<KillerEffectObject>().InnerMirror = false;
        }

        kill = false;
        entermirror = false;
        upstairs = false;
        downstairs = false;
    }

    private IEnumerator AnimCoroutine()
    {
        Animator killanim = GetComponent<Animator>();
        killanim.SetBool("kill", true);
        GetComponent<PlayerMove>().speed = 0;
        yield return new WaitForSeconds(3.0f);
        killanim.SetBool("kill", false);
        gamemanagerscript.Clear();
        GetComponent<PlayerMove>().speed = 7;
        //Debug.Log(dontmove);
    }
    
}

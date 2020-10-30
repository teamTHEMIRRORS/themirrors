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
    public bool entermirror = false;

    GameObject player;
    PlayerManager playerManager;

    #endregion

    private void Awake()
    {
        if (photonView.IsMine)
        {
            KillerManager.LocalKillerInstance = this.gameObject;
        }

        //DontDestroyOnLoad(this.gameObject);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hitplayer");
            kill = true;
            player = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("Mirror"))
        {
            Debug.Log("hitMirror");
            entermirror = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("you can't kill player");
            kill = false;
        }
        else if (collision.gameObject.CompareTag("Mirror"))
        {
            Debug.Log("you can't enter mirror");
            entermirror = false;
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
            Animator killanim = GetComponent<Animator>();
            killanim.SetBool("kill", true);
            playerManager = player.GetComponent<PlayerManager>();
            //playerManager.HP -= 1000;
            playerManager.Damageplayer();
            killanim.SetBool("kill", false);
        }
        else if (entermirror)
        {

        }

        kill = false;
        entermirror = false;
    }

    
}

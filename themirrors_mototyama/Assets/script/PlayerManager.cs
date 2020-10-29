using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class PlayerManager : MonoBehaviourPunCallbacks//,IPunObservable
{
    #region Public Field

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public int HP = 100;
    public GameObject gamemanager;
    GameManager gamemanagerscript;

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

    private void Update()
    {
        if(HP <= 0 && photonView.IsMine)
        {
            gamemanagerscript.LeaveRoom();
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


    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 0f, 0f);
        }
    }
    #endregion

    public void SurviorAction()
    {

    }


    //public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    //{
    //if (stream.IsWriting)
    //{
    //stream.SendNext(this.HP);
    //}
    //else
    //{
    //this.HP = (int)stream.ReceiveNext();
    //}
    //}

    public void Damageplayer()
    {
        photonView.RPC("KilledPlayer", RpcTarget.All, this.HP);
    }

    [PunRPC]
    void KilledPlayer(int HP)
    {

        this.HP -= 100;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Field

    public static GameManager Instance;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    public GameObject killerPrefab;
    public GameObject bottunPrefab;
    public static GameObject player;
    public GameObject selectcanvas;
    public GameObject startcanbus;
    public GameObject cameramanager;
    public GameObject[] survivors;
    public GameObject killer;

    public bool killerexist = false;
    public static string playerrole;
    public int playercounter = 0;
    public int maxplayer;

    //PlayerInstance playerinstance;

    #endregion

    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    

    

    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    public void CreateCharacter()
    {
        Instance = this;

        startcanbus.SetActive(true);
        GameObject uilist = GameObject.Find("UIList");

        if (playerrole == "survivor")
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                    //player = Instantiate(playerPrefab);

                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }

                if (UIManager.bottunList == null)
                {
                    

                    GameObject buttun = PhotonNetwork.Instantiate(this.bottunPrefab.name, this.bottunPrefab.transform.position, Quaternion.identity, 0);
                    //GameObject buttun = Instantiate(bottunPrefab);
                    buttun.transform.SetParent(uilist.transform);


                    //RightBottun rightBottun = buttun.GetComponentInChildren<RightBottun>();
                    //rightBottun.AddEventRight();
                }
            }
        }
        else if(playerrole == "killer")
        {
            if (killerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> killerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    if(killerexist == false)
                    {
                        player = PhotonNetwork.Instantiate(this.killerPrefab.name, new Vector3(10f, 0f, 0f), Quaternion.identity, 0);
                    }
                    //player = PhotonNetwork.Instantiate(this.killerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                    //player = Instantiate(playerPrefab);

                    killerexist = true;

                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }

                if (UIManager.bottunList == null)
                {
                    

                    GameObject buttun = PhotonNetwork.Instantiate(this.bottunPrefab.name, this.bottunPrefab.transform.position, Quaternion.identity, 0);
                    //GameObject buttun = Instantiate(bottunPrefab);
                    buttun.transform.SetParent(uilist.transform);


                    //RightBottun rightBottun = buttun.GetComponentInChildren<RightBottun>();
                    //rightBottun.AddEventRight();
                }
            }
        }
        
    }

    public void DecideSurvivorRole()
    {
        playerrole = "survivor";
        //playerinstance.PlayerNumber(playercounter);
        //playercounter += 1;
        CreateCharacter();
        //Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        //selectcanvas.SetActive(false);
        Instantiate(cameramanager);
    }

    public void DecideKillerRole()
    {
        if(killer == null)
        {
            playerrole = "killer";
            CreateCharacter();
            Instantiate(cameramanager);
        }
        else
        {
            return;
        }
        //playerrole = "killer";
        //playerinstance.PlayerNumber(playercounter);
        //playercounter += 1;
        //CreateCharacter();
        //Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        //selectcanvas.SetActive(false);
        //Instantiate(cameramanager);
    }


    #endregion

    #region Private Methods


    

    private void Start()
    {
        //Instance = this;
        //GameObject uilist = GameObject.Find("UIList");

        //if (playerPrefab == null)
        //{
        //Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        //}
        //else
        //{
        //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        //if (PlayerManager.LocalPlayerInstance == null)
        //{
        //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        //player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        //player = Instantiate(playerPrefab);

        //}
        //else
        //{
        //Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        //}

        //if (UIManager.bottunList == null)
        //{
        //GameObject buttun = PhotonNetwork.Instantiate(this.bottunPrefab.name, this.bottunPrefab.transform.position, Quaternion.identity, 0);
        //GameObject buttun = Instantiate(bottunPrefab);
        //buttun.transform.SetParent(uilist.transform);


        //RightBottun rightBottun = buttun.GetComponentInChildren<RightBottun>();
        //rightBottun.AddEventRight();
        //}
        //}

        //CreateCharacter();
    }

    private void Update()
    {

        if(maxplayer != 5)
        {
            survivors = GameObject.FindGameObjectsWithTag("Player");
            killer = GameObject.FindGameObjectWithTag("Killer");
            if(killer == null)
            {
                playercounter = survivors.Length;
            }
            else
            {
                playercounter = survivors.Length + 1;
            }
            maxplayer = PhotonNetwork.CurrentRoom.PlayerCount;
        }

        if(playercounter == maxplayer)
        {
            selectcanvas.SetActive(false);
            //startcanbus.SetActive(true);

        }
    }

    private void Awake()
    {
        
        //playerinstance = this.GetComponent<PlayerInstance>();
        
    }

    #endregion
}

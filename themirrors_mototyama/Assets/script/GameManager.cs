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
    public GameObject camera;
    //public GameObject cameramanager;
    public GameObject[] survivors;
    public GameObject killer;

    public bool killerexist = false;
    public static string playerrole;
    public int playercounter = 0;
    //public int maxplayer;
    public int breakedmirror = 0;
    public int killedplayer = 0;
    public float time = 0;
    int c = 0;
    

    //PlayerInstance playerinstance;


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
        //GameObject uilist = GameObject.Find("UIList");

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
                    player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 1.35f, 0f), Quaternion.identity, 0);
                    photonView.RPC("PlayerCountUp", RpcTarget.All,killerexist);
                    //player = Instantiate(playerPrefab);

                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }

                //if (UIManager.bottunList == null)
                
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
                        player = PhotonNetwork.Instantiate(this.killerPrefab.name, new Vector3(0f, 81.15f, 0f), Quaternion.identity, 0);
                        killerexist = true;
                    }
                    //player = PhotonNetwork.Instantiate(this.killerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                    //player = Instantiate(playerPrefab);
                    photonView.RPC("PlayerCountUp", RpcTarget.All,killerexist);
                    

                }
                else
                {
                    //Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }

                //if (UIManager.bottunList == null)
                
            }
        }

        
        
    }

    public void DecideSurvivorRole()
    {
        CountPlayer();
        playerrole = "survivor";
        Debug.Log(playerrole);
        //playerinstance.PlayerNumber(playercounter);
        //playercounter += 1;
        CreateCharacter();
        //Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        camera.transform.parent = player.transform;

        //selectcanvas.SetActive(false);
        //Instantiate(cameramanager);
    }

    public void DecideKillerRole()
    {
        if(!killerexist)
        {
            CountPlayer();
            playerrole = "killer";
            Debug.Log(playerrole);
            CreateCharacter();
            //Instantiate(cameramanager);
            
            float cameraposx = player.transform.position.x;
            //float cameraposy = player.transform.position.y;
            camera.transform.position = new Vector3(0f,82.65f,-10f);
            camera.transform.parent = player.transform;
            //selectcanvas.SetActive(false);
        }
        else
        {
            return;
        }
        
    }

    public void Clear()
    {
        if (breakedmirror == 4)
        {
            if (playerrole == "survivor")
            {
                SceneManager.LoadScene("surv_win");
                LeaveRoom();
            }

            if(playerrole == "killer")
            {
                SceneManager.LoadScene("killer_lose");
                LeaveRoom();
            }

        }
        else if (killedplayer == playercounter - 1 && playercounter > 1)
        {
            if (playerrole == "killer")
            {
                SceneManager.LoadScene("killer_win");
                LeaveRoom();
            }
            else
            {
                SceneManager.LoadScene("surv_lose");
                LeaveRoom();
            }
        }
    }


    #endregion

    #region Private Methods

    private void Awake()
    {

        //playerinstance = this.GetComponent<PlayerInstance>();
        camera = GameObject.Find("Camera");
        

    }

    private void Update()
    {
        
        if (playercounter == PhotonNetwork.CurrentRoom.PlayerCount && killerexist)
        {
            if(playercounter > 1)
            {
                //Debug.Log(playercounter);
                selectcanvas.SetActive(false);
                //time = 0;
                SetBottun();
                //c++;
            }
            
        }

        if(time > 360.0f)
        {
            if (playerrole == "killer")
            {
                SceneManager.LoadScene("killer_lose");
                LeaveRoom();
            }
            if (playerrole == "survivor")
            {
                SceneManager.LoadScene("surv_lose");
                LeaveRoom();
            }
        }
        if(UIManager.bottunList != null)
        {
            time += Time.deltaTime;
        }
        
    }

    private void SetBottun()
    {
        if (UIManager.bottunList == null)
        {
            GameObject uilist = GameObject.Find("UIList");

            GameObject buttun = PhotonNetwork.Instantiate(this.bottunPrefab.name, this.bottunPrefab.transform.position, Quaternion.identity, 0);
            //GameObject buttun = Instantiate(bottunPrefab);
            buttun.transform.SetParent(uilist.transform);
            time = 0;
        }
    }
    
    private void CountPlayer()
    {
        killer = GameObject.Find("Killer(Clone)");
        survivors = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(survivors.Length);
        Debug.Log(killer);
        if (killer == null)
        {
            playercounter = survivors.Length;
        }
        else
        {
            killerexist = true;
            playercounter = survivors.Length + 1;
        }
        Debug.Log(killerexist);
    }

    #endregion

    [PunRPC]
    void PlayerCountUp(bool exist)
    {
        killerexist = exist;
        this.playercounter += 1; 
    }
}

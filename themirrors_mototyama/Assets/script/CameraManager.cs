using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraManager : MonoBehaviour
{
    int playercounter = 0;
    GameObject[] cameras;
    PhotonView photonview;

    

    // Update is called once per frame
    void Update()
    {
        if(playercounter < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            CameraDestoroy();
            
        }
    }

    private void Awake()
    {
        playercounter = 0;
    }

    void CameraDestoroy()
    {
        cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        //Debug.Log(cameras.Length);
        if(cameras.Length > 1)
        {
            for(int i = 0; i < cameras.Length; i++)
            {
                photonview = cameras[i].GetComponentInParent<PhotonView>();
                if (!photonview.IsMine)
                {
                    Destroy(cameras[i]);
                }
            }

            playercounter = PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }

}

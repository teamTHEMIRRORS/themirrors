using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class RightBottun : MonoBehaviour
{
    

    private void Awake()
    {
        AddEventRight();
    }

    public void AddEventRight()
    {

        Debug.Log("AddEvent");
        //GameObject player = GameManager.player;
        //PlayerMove push = player.GetComponent<PlayerMove>();


        EventTrigger bottunTrigger = GetComponent<EventTrigger>();
        //bottunTrigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => DownRightBottun());
        bottunTrigger.triggers.Add(entry);


        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => UpRightBottun());
        bottunTrigger.triggers.Add(entry);
    }

    void DownRightBottun()
    {
        GameObject player = GameManager.player;
        PlayerMove push = player.GetComponent<PlayerMove>();
        push.PushDownRight();
    }

    void UpRightBottun()
    {
        GameObject player = GameManager.player;
        PlayerMove push = player.GetComponent<PlayerMove>();
        push.PushUp();
    }
}

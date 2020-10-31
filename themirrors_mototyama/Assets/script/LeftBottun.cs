using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftBottun : MonoBehaviour
{
    private int c = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (c == 0)
        {
            AddEventLeft();
            c++;
        }
    }

    public void AddEventLeft()
    {

        Debug.Log("AddEvent");
        //GameObject player = GameManager.player;
        //PlayerMove push = player.GetComponent<PlayerMove>();


        EventTrigger bottunTrigger = GetComponent<EventTrigger>();
        //bottunTrigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => DownLeftBottun());
        bottunTrigger.triggers.Add(entry);


        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => UpLeftBottun());
        bottunTrigger.triggers.Add(entry);
    }

    void DownLeftBottun()
    {
        GameObject player = GameManager.player;
        PlayerMove push = player.GetComponent<PlayerMove>();
        push.PushDownLeft();
    }

    void UpLeftBottun()
    {
        GameObject player = GameManager.player;
        PlayerMove push = player.GetComponent<PlayerMove>();
        push.PushUp();
    }
}

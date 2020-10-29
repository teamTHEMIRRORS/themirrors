using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ActionBottun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        AddActionEvent();
    }

    public void AddActionEvent()
    {
        Debug.Log("AddActionEvent");


        Button bottunTrigger = GetComponent<Button>();
        //bottunTrigger.triggers = new List<EventTrigger.Entry>();

        bottunTrigger.onClick.AddListener(Action);

    }

    void Action()
    {
        Debug.Log("action");
        GameObject player = GameManager.player;
        string role = GameManager.playerrole;

        if(role == "survivor")
        {
            PlayerManager action = player.GetComponent<PlayerManager>();
            action.SurviorAction();
        }
        else if(role == "killer")
        {
            KillerManager action = player.GetComponent<KillerManager>();
            action.KillerAction();
        }
    }
}

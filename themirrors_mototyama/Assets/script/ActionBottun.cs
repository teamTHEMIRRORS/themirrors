using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class ActionBottun : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI _actionTextMeshProUGUI;
    private 
    void Start()
    {
        _actionTextMeshProUGUI = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        currentAction();
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

    void currentAction()
    {
        GameObject player = GameManager.player;
        string role = GameManager.playerrole;

        if(role == "survivor")
        {
            PlayerManager action = player.GetComponent<PlayerManager>();
            if (action.breakmirror)
            {
                _actionTextMeshProUGUI.text = "Break The Mirror!";
            }
            else if (action.upstairs)
            {
                _actionTextMeshProUGUI.text = "go upstairs";
            }
            else if (action.downstairs)
            {
                _actionTextMeshProUGUI.text = "go downstairs";
            }
            else
            {
                _actionTextMeshProUGUI.text = "no action";
            }
        }
        else if(role == "killer")
        {
            KillerManager action = player.GetComponent<KillerManager>();
            if (action.kill)
            {
                _actionTextMeshProUGUI.text = "Kill The Surviver!";
            }
            else if (action.entermirror)
            {
                _actionTextMeshProUGUI.text = "Enter Mirror";
            }
            else if (action.upstairs)
            {
                _actionTextMeshProUGUI.text = "go upstairs";
            }
            else if (action.downstairs)
            {
                _actionTextMeshProUGUI.text = "go downstairs";
            }
            else if (action.exitmirrorworld)
            {
                _actionTextMeshProUGUI.text = "Exit Mirror";
            }
            else
            {
                _actionTextMeshProUGUI.text = "no action";
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBreak : MonoBehaviour
{
    bool breakflag = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            breakflag = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        breakflag = false;
        Debug.Log(breakflag);
    }

    public void Onclick()
    {
        if (breakflag)
        {
            this.gameObject.SetActive(false);
            breakflag = false;
        }
    }

}

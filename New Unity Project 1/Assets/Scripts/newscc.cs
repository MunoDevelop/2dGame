using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class newscc : MonoBehaviour {
    public GameObject mask;
    public GameObject uicamera;
    public GameObject UIeventsystem;

     bool uion = false;

   
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyUp("tab")){
            if (uion==false)
            {
                mask.GetComponent<SpriteRenderer>().enabled = true;
                uicamera.GetComponent<Camera>().enabled = true;
                UIeventsystem.GetComponent<EventSystem>().enabled = true;
                Time.timeScale = 0;
                uion = true;
            }else 
            {
                mask.GetComponent<SpriteRenderer>().enabled = false;
                uicamera.GetComponent<Camera>().enabled = false;
                UIeventsystem.GetComponent<EventSystem>().enabled = false;
                Time.timeScale = 1;
                uion = false;
            }
        }

		
	}
}

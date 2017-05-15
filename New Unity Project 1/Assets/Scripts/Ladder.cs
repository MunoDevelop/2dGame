using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

    // Use this for initialization
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponentInParent<PlayerController>().LadderTriggerSetting(true);
        other.GetComponentInParent<PlayerController>().LadderTriggerXpositionSetting(this.transform.position.x);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponentInParent<PlayerController>().LadderTriggerSetting(false);
    }
}

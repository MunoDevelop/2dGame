using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderTrigger : MonoBehaviour {
    [SerializeField]
    private BoxCollider2D boxcollider;
    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collision)
    {
        boxcollider.isTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        boxcollider.isTrigger = false;
    }
}

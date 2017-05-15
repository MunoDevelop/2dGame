using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.AI;


public class ButtonEvent : MonoBehaviour
{
    private Material mat;
    public Color PointerEnterColor;
    public Color PointerExitColor;
    //private Color PointerEnterColorExist;
    //private Color PointerExitColorExist;

    public void PointerEnter()
    {
        Material newmat = Instantiate(mat);
        newmat.SetColor("_Color", PointerEnterColor);
        GetComponentInParent<Image>().material = newmat;
    }
    public void PointerExit()
    {
        Material newmat = Instantiate(mat);
        newmat.SetColor("_Color", PointerExitColor);
        GetComponentInParent<Image>().material = newmat;
    }
    void Start()
    {
        mat = GetComponentInParent<Image>().material;
        //PointerEnterColorExist = PointerEnterColor;
        //PointerExitColorExist = PointerExitColor;
       
    }

    // Update is called once per frame
    void Update () {
		
	}
}

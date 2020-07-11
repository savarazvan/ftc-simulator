using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammingControls : MonoBehaviour
{
    public GameObject programmingUI, buildUI;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            programmingUI.SetActive(!programmingUI.activeSelf);
            buildUI.SetActive(!buildUI.activeSelf);
        }        
    }
}

using UnityEngine;
using System;

public class ProgrammingControls : MonoBehaviour
{
    public Canvas programmingUI, buildUI;

    private void Start() {
        UIDrag.separator = programmingUI.transform.Find
            ("Separator").GetComponent<RectTransform>().localPosition.x;
        programmingUI.enabled=false;
        buildUI.enabled=true;    
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            programmingUI.enabled = !programmingUI.enabled;
            buildUI.enabled = !buildUI.enabled;
        }
    }
}

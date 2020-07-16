using UnityEngine;
using System;

public class ProgrammingControls : MonoBehaviour
{
    public Canvas programmingUI, buildUI;
    public bool cursorLocked, liveMode;
    private void Start()
    {
        UIDrag.separator = programmingUI.transform.Find
            ("Separator").GetComponent<RectTransform>().localPosition.x;
        programmingUI.enabled = false;
        buildUI.enabled = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (liveMode)
            {
                Cursor.lockState = cursorLocked ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = !cursorLocked;
                cursorLocked = !cursorLocked;
            }
            programmingUI.enabled = !programmingUI.enabled;
            if (buildUI != null)
                buildUI.enabled = !buildUI.enabled;
        }
    }
}

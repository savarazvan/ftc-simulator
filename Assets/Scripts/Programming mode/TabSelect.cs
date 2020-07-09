using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSelect : MonoBehaviour
{
    public GameObject[] tabs;
    public static GameObject current;
    // Start is called before the first frame update
    void Start()
    {
        current = tabs[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectTab(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectTab(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            selectTab(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            selectTab(3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            selectTab(4);
    }

    void selectTab(int tab)
    {
        if(tabs[tab]==current) return;
        current.SetActive(false);
        current = tabs[tab];
        current.SetActive(true);
    }
}

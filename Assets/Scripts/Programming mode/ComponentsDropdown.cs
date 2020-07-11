using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentsDropdown : MonoBehaviour
{
    public string dropDownTag;
    public Dictionary<int, GameObject> dictionary;
    private List<string> newOptions;
    private Dropdown dropdown;
    private void Start()
    {
        updateList();
    }

    private void OnEnable() {
        updateList();
    }

    public void updateList()
    {
        dropdown = GetComponent<Dropdown>();
        newOptions = new List<string>();
        dropdown.ClearOptions();
        dictionary = new Dictionary<int, GameObject>();
        GameObject[] obj = GameObject.FindGameObjectsWithTag(dropDownTag);
        for (int i=0; i<obj.Length; i++)
        {
            dictionary.Add(i, obj[i]);
            newOptions.Add(obj[i].name);
        }
        dropdown.AddOptions(newOptions);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopulateInputKeys : MonoBehaviour
{
    private string[] keys = {
 "backspace",
 "delete",
 "tab",
 "clear",
 "return",
 "pause",
 "escape",
 "space",
 "up",
 "down",
 "right",
 "left",
 "insert",
 "home",
 "end",
 "page up",
 "page down",
 "f1",
 "f2",
 "f3",
 "f4",
 "f5",
 "f6",
 "f7",
 "f8",
 "f9",
 "f10",
 "f11",
 "f12",
 "f13",
 "f14",
 "f15",
 "0",
 "1",
 "2",
 "3",
 "4",
 "5",
 "6",
 "7",
 "8",
 "9",
 "!",
 "\"",
 "#",
 "$",
 "&",
 "'",
 "(",
 ")",
 "*",
 "+",
 ",",
 "-",
 ".",
 "/",
 ":",
 ";",
 "<",
 "=",
 ">",
 "?",
 "@",
 "[",
 "\\",
 "]",
 "^",
 "_",
 "`",
 "a",
 "b",
 "c",
 "d",
 "e",
 "f",
 "g",
 "h",
 "i",
 "j",
 "k",
 "l",
 "m",
 "n",
 "o",
 "p",
 "q",
 "r",
 "s",
 "t",
 "u",
 "v",
 "w",
 "x",
 "y",
 "z",
 "numlock",
 "caps lock",
 "scroll lock",
 "right shift",
 "left shift",
 "right ctrl",
 "left ctrl",
 "right alt",
 "left alt"
    };
    private List<string> options = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        Dropdown dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        foreach(string key in keys)
            options.Add(key);
        dropdown.AddOptions(options);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

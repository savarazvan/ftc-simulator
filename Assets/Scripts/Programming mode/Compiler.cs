﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
public class Compiler : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void sendMessage(string message);
#endif

    public Transform mainLoop;
    public static string path;
    private string lastMessage = null;
    Dictionary<string, int> intVars;
    Dictionary<string, float> floatVars;

    void Start()
    { }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) run();
    }

    public void run()
    {
        intVars = new Dictionary<string, int>();
        floatVars = new Dictionary<string, float>();
        StartCoroutine(generateCode(mainLoop));
    }


    public IEnumerator generateCode(Transform obj)
    {
        foreach (Transform child in obj)
        {
            switch (child.tag)
            {
                case ("Loop"):
                    {
                        handleLoop(child, child.GetChild(1));
                        break;
                    }

                case ("Variable"):
                    {
                        addVariable(child);
                        break;
                    }

                case ("Operator"):
                    {
                        handleOperation(child);
                        break;
                    }

                case ("RobotFunction"):
                    {
                        handleRobotFunc(child);
                        break;
                    }

                case ("Condition"):
                    {
                        if (handleCondition(child.GetChild(1)))
                            StartCoroutine(generateCode(child));
                        break;
                    }
            }
        }
        yield return true;
    }


    public bool handleCondition(Transform block)
    {
        string type = block.Find("Text").GetComponent<Text>().text;
        InputField[] vars = block.GetComponentsInChildren<InputField>();
        switch (type)
        {
            case "==":
                {
                    if (getVarType(vars[0].text))
                    {
                        if (intVars[vars[0].text] == Int32.Parse(vars[1].text))
                            return true;
                        else if (intVars.ContainsKey(vars[1].text) && intVars[vars[0].text] == intVars[vars[1].text])
                            return true;
                        return false;
                    }

                    else
                    {
                        if (floatVars[vars[0].text] == float.Parse(vars[1].text))
                            return true;
                        if (floatVars.ContainsKey(vars[1].text) && floatVars[vars[0].text] == floatVars[vars[1].text])
                            return true;
                        return false;
                    }
                }
            case "!=":
                {
                    if (getVarType(vars[0].text))
                    {
                        if (intVars[vars[0].text] != Int32.Parse(vars[1].text))
                            return true;
                        else if (intVars.ContainsKey(vars[1].text) && intVars[vars[0].text] != intVars[vars[1].text])
                            return true;
                        return false;
                    }

                    else
                    {
                        if (floatVars[vars[0].text] != float.Parse(vars[1].text))
                            return true;
                        if (floatVars.ContainsKey(vars[1].text) && floatVars[vars[0].text] != floatVars[vars[1].text])
                            return true;
                        return false;
                    }
                }
            default: return false;
        }
    }

    public void handleLoop(Transform loop, Transform condition)
    {
        while (!handleCondition(condition))
            StartCoroutine(generateCode(loop));
    }

    public void addVariable(Transform varBlock)
    {
        string type = varBlock.GetComponentInChildren<Text>().text;
        string name = varBlock.GetComponentInChildren<InputField>().text;
        if (type == "int")
            intVars.Add(name, 0);
        else floatVars.Add(name, 0);
    }

    public void handleOperation(Transform opBlock)
    {
        string type = opBlock.Find("Text").GetComponent<Text>().text;
        InputField[] vars = opBlock.GetComponentsInChildren<InputField>();
        switch (type)
        {
            case "=":
                {
                    if (intVars.ContainsKey(vars[0].text))
                    {
                        if (intVars.ContainsKey(vars[1].text))
                            intVars[vars[0].text] = intVars[vars[1].text];
                        else intVars[vars[0].text] = Int32.Parse(vars[1].text);
                        Debug.Log(intVars[vars[0].text]);
                    }
                    else if (floatVars.ContainsKey(vars[0].text))
                    {
                        if (floatVars.ContainsKey(vars[1].text))
                            floatVars[vars[0].text] = floatVars[vars[1].text];
                        else floatVars[vars[0].text] = float.Parse(vars[1].text);
                    }
                    break;
                }

            case "+=":
                {
                    if (intVars.ContainsKey(vars[0].text))
                    {
                        if (intVars.ContainsKey(vars[1].text))
                            intVars[vars[0].text] += intVars[vars[1].text];
                        else intVars[vars[0].text] += Int32.Parse(vars[1].text);
                    }
                    else if (floatVars.ContainsKey(vars[0].text))
                    {
                        if (floatVars.ContainsKey(vars[1].text))
                            floatVars[vars[0].text] += floatVars[vars[1].text];
                        else floatVars[vars[0].text] += float.Parse(vars[1].text);
                    }
                    break;
                }

            case "-=":
                {
                    if (intVars.ContainsKey(vars[0].text))
                    {
                        if (intVars.ContainsKey(vars[1].text))
                            intVars[vars[0].text] -= intVars[vars[1].text];
                        else intVars[vars[0].text] -= Int32.Parse(vars[1].text);
                    }
                    else if (floatVars.ContainsKey(vars[0].text))
                    {
                        if (floatVars.ContainsKey(vars[1].text))
                            floatVars[vars[0].text] -= floatVars[vars[1].text];
                        else floatVars[vars[0].text] -= float.Parse(vars[1].text);
                    }
                    break;
                }

            case "*=":
                {
                    if (intVars.ContainsKey(vars[0].text))
                    {
                        if (intVars.ContainsKey(vars[1].text))
                            intVars[vars[0].text] *= intVars[vars[1].text];
                        else intVars[vars[0].text] *= Int32.Parse(vars[1].text);
                    }
                    else if (floatVars.ContainsKey(vars[0].text))
                    {
                        if (floatVars.ContainsKey(vars[1].text))
                            floatVars[vars[0].text] *= floatVars[vars[1].text];
                        else floatVars[vars[0].text] *= float.Parse(vars[1].text);
                    }
                    break;
                }

            case "/=":
                {
                    if (intVars.ContainsKey(vars[0].text))
                    {
                        if (intVars.ContainsKey(vars[1].text))
                            intVars[vars[0].text] /= intVars[vars[1].text];
                        else intVars[vars[0].text] /= Int32.Parse(vars[1].text);
                    }

                    else if (floatVars.ContainsKey(vars[0].text))
                    {
                        if (floatVars.ContainsKey(vars[1].text))
                            floatVars[vars[0].text] /= floatVars[vars[1].text];
                        else floatVars[vars[0].text] /= float.Parse(vars[1].text);
                    }
                    break;
                }
        }
    }

    public void handleRobotFunc(Transform func)
    {
        string type = func.Find("Text").GetComponent<Text>().text;
        float value = float.Parse(func.GetComponentInChildren<InputField>().text);

        switch (type)
        {
            case "Set motor power":
                {
                    Dropdown dropdown = func.GetComponentInChildren<Dropdown>();
                    ComponentsDropdown componentsDropdown = dropdown.GetComponent<ComponentsDropdown>();
                    GameObject selectedMotor = componentsDropdown.dictionary[dropdown.value];
                    selectedMotor.GetComponent<Motor>().setPower(value);
                    break;
                }

            case "Sleep":
                {
                    sleep(value);
                    break;
                }
        }
    }

    public bool getVarType(string var)
    {
        if (intVars.ContainsKey(var))
            return true;
        return false;
    }
    IEnumerator sleep(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}




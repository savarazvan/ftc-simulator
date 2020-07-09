using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GenerateCode : MonoBehaviour
{
    
    public Transform mainLoop;
    public static string path;
    void Start()
    {
        path = Application.dataPath + "/code.txt";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            File.WriteAllText(path, "");
            generateCode(mainLoop);
        }
    }

    public void generateCode(Transform obj)
    {
        foreach(Transform child in obj)
        {
            switch(child.tag)
            {
                case ("Loop"):
                {
                    string type = child.GetComponentInChildren<Text>().text;
                    string cond = child.GetComponentInChildren<InputField>().text;
                    File.AppendAllText(path, type + '('+cond +") \n{\n");
                    generateCode(child);
                    File.AppendAllText(path, "\n}\n");
                    break;
                }

                case ("Variable"):
                {
                    string type = child.GetComponentInChildren<Text>().text;
                    string name = child.GetComponentInChildren<InputField>().text;
                    File.AppendAllText(path, type + ' ' + name +";\n");
                    break;
                }

                case ("Operator"):
                {
                    string type = child.Find("Text").GetComponent<Text>().text;
                    InputField[] vars = child.GetComponentsInChildren<InputField>();
                    File.AppendAllText(path, vars[0].text + type + vars[1].text + ";\n");
                    break;
                }

                case ("BotFunction"):
                {
                    string input = child.GetComponentInChildren<InputField>().text;
                    if(child.Find("Bot send message") == null)
                    {
                        File.AppendAllText(path, input + " = ChatBot.readLastMessage();\n");
                        break;
                    }

                    File.AppendAllText(path, "ChatBot.sendMessage(\"" + input + "\");\n");
                    break;
                }

                default: break;
                
            }
        }
    }
}

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class SaveAndLoadRobot : MonoBehaviour
{
    public GameObject block, motor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) saveRobot();
    }

    private void saveRobot()
    {
        Transform robot = GameObject.Find("Robot").transform;
        AllRobotComponents allComponents = new AllRobotComponents();
        foreach (Transform component in robot)
        {
            switch (component.tag)
            {
                case ("Building/Block"):
                    {
                        allComponents.blocks.Add(new Block(component));
                        break;
                    }
                case ("Building/Wheel"):
                    {
                        allComponents.motors.Add(new MotorAndWheel(component));
                        break;
                    }
            }
        }
        //-----------------------------------------------------------------------------
        string path = "Assets/Resources/robot.xml";
        var writer = new System.Xml.Serialization.XmlSerializer(typeof(AllRobotComponents));  
        var file = new System.IO.StreamWriter(path);
        writer.Serialize(file, allComponents);
        file.Close();
        print("Saved to " + path);
    }
}

public class AllRobotComponents
{
    public List<Block> blocks;
    public List<MotorAndWheel> motors;
    public AllRobotComponents()
    {
        blocks = new List<Block>();
        motors = new List<MotorAndWheel>();
    }
}
public class Block
{
    public Vector3[] transformValues = new Vector3[3];
    public string objectName;
    public Block(Transform transformComponent)
    {
        transformValues[0] = transformComponent.position;
        transformValues[1] = transformComponent.eulerAngles;
        transformValues[2] = transformComponent.localScale;
        objectName = transformComponent.name;
    }
    public Block(){}
    public Vector3 getTransform(int value)
    {
        return transformValues[value];
    }

    public string getName()
    {
        return objectName;
    }
}

public class MotorAndWheel : Block
{
    public MotorAndWheel(Transform transformComponent) : base(transformComponent)
    {
        connectedBody = transformComponent.GetComponentInChildren<HingeJoint>().connectedBody.name;
    }

    public MotorAndWheel(){}

    public string connectedBody;
}


using UnityEngine;
using UnityEditor;
using System.IO;

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
        
    }

    private void saveRobot()
    {
        string path = "Assets/Resources/robot.txt";
        StreamWriter writer = new System.IO.StreamWriter(path, true);
        Transform robot = GameObject.Find("Robot").transform;
        foreach(Transform component in robot)
        {
            switch(component.tag)
            {
                case("Building/Block"):
                {
                    Block block = new Block(component);
                    var blockWriter = new System.Xml.Serialization.XmlSerializer(typeof(Block));
                    blockWriter.Serialize(writer,block);
                    break;
                }
                case("Building/Wheel"):
                {
                    MotorAndWheel motor = new MotorAndWheel(component);
                    var motorWriter = new System.Xml.Serialization.XmlSerializer(typeof(MotorAndWheel));
                    motorWriter.Serialize(writer,motor);
                    break;
                }
                
            }
        }
        writer.WriteLine("Test");
        writer.Close();
    }
}

public class Block : SaveAndLoadRobot
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

    public virtual GameObject create()
    {
        GameObject newBlock = block;
        block.transform.position = transformValues[0];
        block.transform.eulerAngles = transformValues[1];
        block.transform.localScale = transformValues[2];
        newBlock.name = objectName;
        return newBlock;
    }
}

public class MotorAndWheel : Block
{
    public MotorAndWheel(Transform transformComponent) : base(transformComponent) {
        connectedBody = transformComponent.GetComponentInChildren<HingeJoint>().connectedBody.name;
    }

    public string connectedBody;
    public override GameObject create()
    {
        GameObject newMotor = motor;
        block.transform.position = transformValues[0];
        block.transform.eulerAngles = transformValues[1];
        block.transform.localScale = transformValues[2];
        newMotor.name = objectName;
        return newMotor;
    }
}

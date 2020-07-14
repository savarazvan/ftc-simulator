using UnityEngine;
using System.Collections.Generic;

public class SaveAndLoadRobot : MonoBehaviour
{
    public GameObject block, motor, servo;
    private string path;
    Transform robot;
    public bool liveMode;
    // Start is called before the first frame update
    void Start()
    {
        robot = GameObject.Find("Robot").transform;
        path = "Assets/Resources/robot.xml";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) saveRobot();
        if (Input.GetKeyDown(KeyCode.L)) loadRobot();
    }

    private void saveRobot()
    {
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
                case ("Building/Motor"):
                    {
                        allComponents.motors.Add(new MotorAndWheel(component,
                            component.GetComponentInChildren<HingeJoint>().connectedAnchor));
                        break;
                    }
                case ("Building/ServoBody"):
                    {
                        allComponents.servos.Add(new ServoClasss(component));
                        break;
                    }
            }
        }
        //-----------------------------------------------------------------------------
        var writer = new System.Xml.Serialization.XmlSerializer(typeof(AllRobotComponents));
        var file = new System.IO.StreamWriter(path);
        writer.Serialize(file, allComponents);
        file.Close();
        print("Saved to " + path);
    }

    private void loadRobot()
    {
        foreach (Transform transform in robot)
            Destroy(transform.gameObject);

        var reader = new System.Xml.Serialization.XmlSerializer(typeof(AllRobotComponents));
        var file = new System.IO.StreamReader(path);
        AllRobotComponents robotComponents = (AllRobotComponents)reader.Deserialize(file);
        file.Close();
        handleRobotCreation(robotComponents);
        print("Loaded from " + path);
    }

    //---------------------------------------------------------------------------------------------

    private void handleRobotCreation(AllRobotComponents components)
    {
        foreach (Block blockComp in components.blocks)
        {
            GameObject newBlock = Instantiate(block);
            newBlock = handleTransformAndName(newBlock, blockComp.transformValues, blockComp.objectName);

            if (!liveMode) continue;
            var rb = newBlock.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        foreach (MotorAndWheel motorComp in components.motors)
        {
            GameObject newMotor = Instantiate(motor);
            newMotor = handleTransformAndName(newMotor, motorComp.transformValues, motorComp.objectName);

            string connectedBody;
            if ((connectedBody = motorComp.connectedBody) == null) continue;

            HingeJoint hj = newMotor.GetComponentInChildren<HingeJoint>();
            hj.connectedBody = robot.Find(connectedBody).GetComponent<Rigidbody>();
            hj.connectedAnchor = motorComp.connectedAnchor;

            if (!liveMode) continue;
            hj.GetComponent<Collider>().isTrigger = false;
        }

        foreach (ServoClasss servoComp in components.servos)
        {
            GameObject newServo = Instantiate(servo);
            newServo = handleTransformAndName(newServo, servoComp.transformValues, servoComp.objectName);

            if (servoComp.child == null) continue;
            GameObject newChild = Instantiate(block);
            Destroy(newChild.GetComponent<ObjectCreation>());
            newChild.transform.parent = newServo.transform.Find("Servo").transform;
            newChild.transform.localPosition = servoComp.child.transformValues[0];
            newChild.transform.eulerAngles = servoComp.child.transformValues[1];
            newChild.transform.localScale = servoComp.child.transformValues[2];
            newChild.name = servoComp.child.objectName;
        }
    }

    private GameObject handleTransformAndName(GameObject obj, Vector3[] transformValues, string name)
    {
        obj.transform.parent = robot;
        Destroy(obj.GetComponent<ObjectCreation>());
        obj.transform.localPosition = transformValues[0];
        obj.transform.eulerAngles = transformValues[1];
        obj.transform.localScale = transformValues[2];
        obj.name = name;
        return obj;
    }
}

//---------------------------------------------------------------------

public class AllRobotComponents
{
    public List<Block> blocks;
    public List<MotorAndWheel> motors;
    public List<ServoClasss> servos;
    public AllRobotComponents()
    {
        blocks = new List<Block>();
        motors = new List<MotorAndWheel>();
        servos = new List<ServoClasss>();
    }
}
public class Block
{
    public Vector3[] transformValues = new Vector3[3];
    public string objectName;
    public Block(Transform transformComponent)
    {
        transformValues[0] = transformComponent.localPosition;
        transformValues[1] = transformComponent.eulerAngles;
        transformValues[2] = transformComponent.localScale;
        objectName = transformComponent.name;
    }
    public Block() { }
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

    public string connectedBody;
    public Vector3 connectedAnchor;
    public MotorAndWheel(Transform transformComponent, Vector3 connectedAnchor) : base(transformComponent)
    {
        Rigidbody body;
        if ((body = transformComponent.GetComponentInChildren<HingeJoint>().connectedBody) != null)
            connectedBody = body.name;
        this.connectedAnchor = connectedAnchor;
    }

    public MotorAndWheel() { }

}

public class ServoClasss : Block
{
    public Block child;
    public ServoClasss(Transform transformComponent) : base(transformComponent)
    {
        Transform servoTorque = transformComponent.GetComponentsInChildren<Transform>()[1];
        Transform attachedObject = servoTorque.GetComponentsInChildren<Transform>()[1];
        if (attachedObject == null) return;
        child = new Block(attachedObject);
    }

    public ServoClasss() { }
}


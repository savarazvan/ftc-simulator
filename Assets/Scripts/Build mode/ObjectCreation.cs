using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreation : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Renderer renderer;
    private Color green = new Color(0f, 1f, 0f, 0.4f), red = new Color(1f, 0f, 0f, 0.4f);
    private List<string> validPlacements;
    Vector3 lastMousePosition;
    public float resizeSensitivity = 5f;
    private Material transparent, initialMat;
    private Color initialCol;
    private bool resizable, rotateable;

    private Vector3 rotateVector = new Vector3(90, 0, 0);
    // Start is called before the first frame update
    void Awake()
    {
        transparent = Resources.Load("Materials/Transparent", typeof(Material)) as Material;
    }
    void Start()
    {
        renderer = GetComponent<Renderer>();
        initialMat = renderer.material;
        initialCol = renderer.material.GetColor("_Color");
        renderer.material = transparent;
        GetComponent<Collider>().enabled = false;
        validPlacements = GetComponent<ObjectProprieties>().validPlacements;
        resizable = GetComponent<ObjectProprieties>().resizable;
        rotateable = GetComponent<ObjectProprieties>().rotateable;
    }


    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit)) return;

        float yOffset = 0;
        yOffset = calculateOffset();

        renderer.material.SetColor("_Color", validPlacements.Contains(hit.collider.tag) ? green : red);

        if (Input.GetMouseButtonDown(0) && renderer.material.GetColor("_Color") == green)
        {
            placeObject();
            return;
        }

        if (Input.GetMouseButton(1))
        {
            lockCursor(true);
            if (resizable) handleResize();
        }

        else if (Input.GetMouseButtonUp(1)) lockCursor(false);

        if (Input.mouseScrollDelta.y != 0) handleRotation(Input.mouseScrollDelta.y);

        if (Input.GetKeyDown(KeyCode.Escape))
            Destroy(gameObject);

        transform.position = new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z);
    }

    //------------------------------------------------------------------------------------------

    void placeObject()
    {
        renderer.material = initialMat;
        renderer.material.SetColor("_Color", initialCol);
        Destroy(GetComponent<ObjectCreation>());
        GetComponent<Collider>().enabled = true;
        transform.SetParent(GameObject.Find("Robot").transform);
        //Special wheel condition
        if (gameObject.tag == "Building/Motor")
        {
            HingeJoint hj = GetComponentInChildren<HingeJoint>();
            hj.connectedBody = hit.rigidbody;
            hj.connectedAnchor = hit.transform.InverseTransformPoint(transform.position);
            Vector3 fixedPosition = new Vector3(hj.connectedAnchor.x, hj.connectedAnchor.y, hj.connectedAnchor.z * 1.5f);
            hj.connectedAnchor = fixedPosition;
        }
    }

    void lockCursor(bool mode)
    {
        Cursor.lockState = mode ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !mode;
    }

    void handleResize()
    {
        Vector3 newScale = new Vector3(0, 0, 0);
        if (Input.GetAxis("Mouse X") != 0) newScale.x = resizeSensitivity * Mathf.Sign(Input.GetAxis("Mouse X")) * Time.deltaTime;
        if (Input.GetAxis("Mouse Y") != 0) newScale.z = resizeSensitivity * Mathf.Sign(Input.GetAxis("Mouse Y")) * Time.deltaTime;
        transform.localScale += newScale;
    }

    void handleRotation(float way)
    {
        rotateVector = way > 0 ? new Vector3(rotateVector.z, rotateVector.x, rotateVector.y) :
                                 new Vector3(rotateVector.y, rotateVector.z, rotateVector.x);
        transform.eulerAngles = rotateVector;
    }

    float calculateOffset()
    {
        float offset;
        if (transform.up.x != 0) offset = transform.localScale.x / 2;
        else if (transform.up.y != 0) offset = transform.localScale.y / 2;
        else offset = transform.localScale.z / 2;
        return offset;
    }
}

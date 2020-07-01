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

    public short mouse1Mode;
    private const short MOUSE1_NONE = -1, MOUSE1_RESIZE = 1, MOUSE1_ROTATE = 2;
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

        if (resizable) mouse1Mode = MOUSE1_RESIZE;
        else if (rotateable) mouse1Mode = MOUSE1_ROTATE;
        else mouse1Mode = MOUSE1_NONE;

    }

    private void LateUpdate()
    {
        if (Input.mouseScrollDelta.y < 0 && resizable)
            mouse1Mode = MOUSE1_RESIZE;
        else if (Input.mouseScrollDelta.y > 0 && rotateable)
            mouse1Mode = MOUSE1_ROTATE;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            renderer.material.SetColor("_Color", validPlacements.Contains(hit.collider.tag) ? green : red);

            if (Input.GetMouseButtonDown(0))
            {
                renderer.material = initialMat;
                renderer.material.SetColor("_Color", initialCol);
                Destroy(GetComponent<ObjectCreation>());
                GetComponent<Collider>().enabled = true;
                transform.SetParent(GameObject.Find("Robot").transform);

                //Special wheel condition
                if (gameObject.tag == "Building/Wheel")
                {
                    HingeJoint hj = GetComponentInChildren<HingeJoint>();
                    hj.connectedBody = hit.rigidbody;
                    hj.connectedAnchor = hit.transform.InverseTransformPoint(transform.position);
                    Vector3 fixedPosition = new Vector3(hj.connectedAnchor.x, hj.connectedAnchor.y, hj.connectedAnchor.z * 1.5f);
                    hj.connectedAnchor = fixedPosition;
                }

                return;
            }

            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                switch (mouse1Mode)
                {
                    case MOUSE1_RESIZE:
                        {
                            Vector3 newScale = new Vector3(0, 0, 0);
                            if (Input.GetAxis("Mouse X") != 0) newScale.x = resizeSensitivity * Mathf.Sign(Input.GetAxis("Mouse X")) * Time.deltaTime;
                            if (Input.GetAxis("Mouse Y") != 0) newScale.z = resizeSensitivity * Mathf.Sign(Input.GetAxis("Mouse Y")) * Time.deltaTime;
                            transform.localScale += newScale;
                            return;
                        }
                    case MOUSE1_ROTATE:
                        {
                            if (Input.GetAxis("Mouse X") != 0)
                            {
                                transform.Rotate(new Vector3(90 * Mathf.Sign(Input.GetAxis("Mouse X")), 0, 0));
                                break;
                            }

                            if (Input.GetAxis("Mouse Y") != 0)
                            {
                                transform.Rotate(new Vector3(0, 90 * Mathf.Sign(Input.GetAxis("Mouse X")), 0));
                                break;
                            }
                            break;
                        }
                    default: break;
                }
            }

            else if (Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                Destroy(gameObject);

            else transform.position = hit.point + Vector3.Scale(transform.up, transform.localScale)/2;
            print(transform.up);
        }

    }
}

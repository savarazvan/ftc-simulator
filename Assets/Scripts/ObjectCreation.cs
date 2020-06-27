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
    private bool resizable;
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
    }

    // Update is called once per frame
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

                //Special wheel exception
                if(gameObject.tag=="Building/Wheel")
                {
                            
                }

                transform.SetParent(hit.transform);
                return;
            }

            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if(!resizable) return;
                Vector3 newScale = new Vector3(0, 0, 0);
                if (Input.GetAxis("Mouse X") != 0) newScale.x = resizeSensitivity * Mathf.Sign(Input.GetAxis("Mouse X")) * Time.deltaTime;
                if (Input.GetAxis("Mouse Y") != 0) newScale.z = resizeSensitivity * Mathf.Sign(Input.GetAxis("Mouse Y")) * Time.deltaTime;
                transform.localScale += newScale;
            }

            else if (Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if(Input.GetKeyDown(KeyCode.Escape))
                Destroy(gameObject);

            else transform.position = hit.point + new Vector3(0, transform.localScale.y / 2, 0);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    private RaycastHit hit;
    public float raycastLenght;
    public Transform objectHeld;
    public float force = 1000;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectHeld)
            {
                dropObject();
                return;
            }

            if (!Physics.Raycast(Camera.main.transform.position, transform.forward, out hit, raycastLenght))
                return;

            if (hit.collider.GetComponent<Rigidbody>() != null)
            {
                objectHeld = hit.collider.transform;
                objectHeld.transform.parent = transform.parent;
                objectHeld.GetComponent<Rigidbody>().isKinematic = true;
                RaycasterPhysicsFix rp = objectHeld.gameObject.AddComponent(typeof(RaycasterPhysicsFix)) as RaycasterPhysicsFix;
                objectHeld.transform.localPosition = new Vector3(0,0,1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && objectHeld)
        {
            objectHeld.transform.parent = null;
            objectHeld.GetComponent<Rigidbody>().isKinematic = false;
            objectHeld.GetComponent<Rigidbody>().AddForce(transform.forward*force,ForceMode.Force);
            objectHeld=null;
        }
    }

    void dropObject()
    {
        objectHeld.transform.parent = null;
        objectHeld.GetComponent<Rigidbody>().isKinematic = false;
        Destroy(objectHeld.GetComponent<RaycasterPhysicsFix>());
        objectHeld = null;
    }

}

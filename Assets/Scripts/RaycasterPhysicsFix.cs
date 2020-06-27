using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasterPhysicsFix : MonoBehaviour
{
    private Vector3 position;
    private void OnCollisionEnter(Collision other) {
        position = transform.position;
        Debug.Log("Collided with "+ other.gameObject.name);
    }
    private void OnCollisionStay(Collision other) {
        transform.position = position;   
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Fence")
            position = transform.position;    
    }
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag=="Fence")
            transform.position = position;
    }
    private void OnCollisionExit(Collision other) {
        transform.localPosition = new Vector3(0,0,1);
    }

    public void OnTriggerExit(Collider other) {
        transform.localPosition = new Vector3(0,0,1);
    }
}

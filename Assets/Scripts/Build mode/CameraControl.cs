using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float cameraSpeed=5f, lookSpeed=5f;
    Vector3 newRot;
    public KeyCode left=KeyCode.A, right=KeyCode.D, forward=KeyCode.W, backward=KeyCode.S, up=KeyCode.LeftShift, down = KeyCode.LeftControl;
    void Update()
    {
        if(Input.GetKey(right)) transform.position += transform.right * cameraSpeed * Time.deltaTime;
        else if(Input.GetKey(left)) transform.position += transform.right * cameraSpeed * -Time.deltaTime;

        if(Input.GetKey(up)) transform.position += transform.up * cameraSpeed * Time.deltaTime;
        else if(Input.GetKey(down)) transform.position += transform.up * -cameraSpeed * Time.deltaTime;

        if(Input.GetKey(forward)) transform.position += transform.forward * Time.deltaTime * cameraSpeed;
        else if(Input.GetKey(backward))  transform.position += transform.forward * Time.deltaTime * -cameraSpeed;

        //----------------------------------------------------------------------------

        if(Input.GetMouseButton(2))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            newRot = transform.eulerAngles;
            if(Input.GetAxis("Mouse X")!=0)
                newRot.y += lookSpeed * Mathf.Sign(Input.GetAxis("Mouse X")) * Time.deltaTime;
            if(Input.GetAxis("Mouse Y")!=0)
                newRot.x -= lookSpeed * Mathf.Sign(Input.GetAxis("Mouse Y")) * Time.deltaTime;
            transform.eulerAngles = newRot;
        }

        else if(Input.GetMouseButtonUp(2))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

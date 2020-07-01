using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    private Rigidbody rb;
    public float power;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)) power+=10;
        if(Input.GetKeyDown(KeyCode.LeftArrow)) power-=10;
        rb.AddTorque(transform.up * power * Time.deltaTime);
    }
    public void setPower(float power)
    {
        this.power=power;
    }
}

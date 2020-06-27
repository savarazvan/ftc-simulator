using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    private Transform axle;
    public float power;
    private void Start()
    {
        axle = GetComponentsInChildren<Transform>()[1];
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)) power+=1;
        if(Input.GetKeyDown(KeyCode.LeftArrow)) power-=1;
        if(power!=0)
            axle.eulerAngles+=new Vector3(power,0,0) * Time.deltaTime;
    }
    public void setPower(float power)
    {
        this.power=power;
    }
}

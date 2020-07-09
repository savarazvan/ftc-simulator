using UnityEngine;

public class Motor : MonoBehaviour
{
    HingeJoint hj;
    private void Start() {
        hj = GetComponentInChildren<HingeJoint>();    
    }
    public void setPower(float power)
    {
        var motor = hj.motor;
        motor.force = power;
        motor.targetVelocity = power;
    }
}

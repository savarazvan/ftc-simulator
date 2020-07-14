using UnityEngine;

public class Servo : MonoBehaviour
{
    [Range(0,180)] public int degree;
     public float turningRate = 5f; 

      private Vector3 targetRotation;

    void Update()
    {
        if(degree==transform.localRotation.z) return;

        targetRotation = new Vector3(0,0,degree);
        transform.eulerAngles = Vector3.Lerp(
            transform.eulerAngles,
            targetRotation,
            turningRate * Time.deltaTime
        );
    }

}

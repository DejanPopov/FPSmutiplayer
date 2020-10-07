using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 

public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocityZ = Vector3.zero;
    private Vector3 rotationZ = Vector3.zero;
    private Vector3 cameraRotationZ = Vector3.zero;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void Move (Vector3 velocity)
    {
        velocityZ = velocity;
    }

    //Gets rotational vector
    public void Rotation(Vector3 rotation)
    {
        rotationZ = rotation;
    }

    public void RotateCamera(Vector3 cameraRotation)
    {
        cameraRotationZ = cameraRotation;
    }


    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        if(velocityZ != Vector3.zero) 
        {
            rb.MovePosition(rb.position + velocityZ * Time.fixedDeltaTime);
        }
    }
    //Perform rotation
    void PerformRotation()
    {
        // Quaternion.Euler vraca rotaciju koja se rotira x stepeni oko x ose, y stepeni oko y ose
        // i z stepeni oko z ose
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationZ));

        if (cam != null)
        {
            cam.transform.Rotate(-cameraRotationZ);
        }
    }
}

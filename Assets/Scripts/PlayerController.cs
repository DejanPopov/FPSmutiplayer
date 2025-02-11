﻿using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Spring settings:")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    //Component cacshing
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        //Adding animation
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }

    void Update()
    {
        
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        // 2 vectors
        Vector3 moveHorizontal = transform.right * xMove;  
        Vector3 moveVertical = transform.forward * zMove;

        // Final movement vector
        Vector3 velocity = (moveHorizontal + moveVertical) * speed;

        //Animate movement
        animator.SetFloat("ForwardVelocity", zMove);

        // Apply movement
        motor.Move(velocity);

        //Calculate rotation as 3D vector
        //Mouse rotation left and right
        //Uvek ocemo da Mouse X utice na pleyera a da Mouse Y utice na kameru.
        float yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, yRotation, 0f) * lookSensitivity;

        //Apply rotation
        motor.Rotation(rotation);

        //Calculate camera rotation as a 3D vector
        float xRotation = Input.GetAxisRaw("Mouse Y");

        float cameraRotation = xRotation * lookSensitivity;

        //Calculate thrust input player
        motor.RotateCamera(cameraRotation);

        //Apply thruster force
        Vector3 thrusterForceV = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            thrusterForceV = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }

        // Apply thruster force
        motor.ApplyThruster(thrusterForceV);
    }

    //Struct
    private void SetJointSettings( float jointSpringV)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = jointSpringV,
            maximumForce = jointMaxForce
        };
    }
}

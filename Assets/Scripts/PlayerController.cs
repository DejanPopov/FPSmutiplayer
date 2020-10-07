using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3;

    private PlayerMotor motor;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        // 2 vectors
        Vector3 moveHorizontal = transform.right * xMove;  
        Vector3 moveVertical = transform.forward * zMove;

        // Final movement vector
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

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

        Vector3 cameraRotation = new Vector3(xRotation , 0f, 0f) * lookSensitivity;

        //Apply camera rotation
        motor.RotateCamera(cameraRotation);
    }
}

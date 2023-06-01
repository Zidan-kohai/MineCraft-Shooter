using UnityEditor;
using UnityEngine;

public class PLayerMovement : MonoBehaviour
{
    [Header("Force")]
    [SerializeField] private float walkForce;
    [SerializeField] private float runForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityForce;

    [Header("Ground Check")]
    [SerializeField] private bool isGround;
    [SerializeField] private float rayLenght;
    [SerializeField] private LayerMask groundLayer;

    private float Yvelocity;
    private CharacterController controller;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Jump();
        Movement();
    }

    private void Movement()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float moveVelocity = walkForce;

        if (Input.GetKey(KeyCode.LeftShift) && direction.x == 0)
        {
            moveVelocity = runForce;
        }

        direction = transform.TransformDirection(direction.normalized);
        direction.y = 0;
        controller.Move(direction * moveVelocity * Time.deltaTime);


        Yvelocity += gravityForce * Time.deltaTime;

        if(isGround && Yvelocity < 0)
        {
            Yvelocity = -2f;
        }

        controller.Move(new Vector3(0, Yvelocity * Time.deltaTime, 0));
    }

    private void Jump()
    {
        isGround = CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGround) 
        {
            Yvelocity =  Mathf.Sqrt(jumpForce * -3f * gravityForce);
        }
    }

    private bool CheckGround()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.green, rayLenght);
        if(Physics.Raycast(transform.position, Vector3.down, out hit, rayLenght, groundLayer)) 
        {
            return true;
        }
        return false;
    }
}

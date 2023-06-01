using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Force")]
    [SerializeField] private float walkForce;
    [SerializeField] private float runForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityForce;

    [Header("Ground Check")]
    [SerializeField] private bool isGround;
    [SerializeField] private float sphereRadius;
    [SerializeField] private float rayLenght;
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private Vector3 ofset;
    [SerializeField] private string[] groundTag;

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
        RaycastHit[] hit;
        hit = Physics.SphereCastAll(groundCheckPosition.position + ofset, sphereRadius, Vector3.down, rayLenght);
        foreach (RaycastHit h in hit)
        {
            foreach (string ground in groundTag)
            {
                if (h.transform.CompareTag(ground))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPosition.position + ofset, sphereRadius);
    }
}

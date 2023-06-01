using UnityEngine;

public class PLayerRotation : MonoBehaviour
{
    [Header("Rotation properties")]
    [SerializeField] private Transform head;
    [SerializeField] private float MouseYIntensivity;
    [SerializeField] private float MouseXIntensivity;
    [SerializeField] private float MinRotateX;
    [SerializeField] private float MaxRotateX;
    private Vector3 originBodyRotation;
    private Vector3 originHeadRotation;

    private void Start()
    {
        originBodyRotation = transform.localEulerAngles;
        originHeadRotation = head.localEulerAngles;
    }

    private void LateUpdate()
    {
        float rotationoY = Input.GetAxis("Mouse X") * MouseYIntensivity;
        float rotationX = -Input.GetAxis("Mouse Y") * MouseXIntensivity;

        originBodyRotation += new Vector3(0, rotationoY,0);
        originHeadRotation += new Vector3(rotationX, 0, 0);

        originHeadRotation.x = Mathf.Clamp(originHeadRotation.x, MinRotateX, MaxRotateX);

        transform.rotation = Quaternion.Euler(originBodyRotation);
        head.rotation = Quaternion.Euler(originBodyRotation + originHeadRotation);
    }
}

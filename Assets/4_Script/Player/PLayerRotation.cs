using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerRotation : MonoBehaviour
{
    [SerializeField] private float MouseYIntensivity;
    [SerializeField] private float MouseXIntensivity;
    [SerializeField] private float MinRotateX;
    [SerializeField] private float MaxRotateX;
    private Vector3 originRotation;
    private void Start()
    {
        originRotation = transform.localEulerAngles;
    }

    private void LateUpdate()
    {
        float rotationoY = Input.GetAxis("Mouse X") * MouseYIntensivity;
        float rotationX = -Input.GetAxis("Mouse Y") * MouseXIntensivity;

        originRotation += new Vector3(rotationX, rotationoY);

        originRotation.x = Mathf.Clamp(originRotation.x, MinRotateX, MaxRotateX);
        transform.rotation = Quaternion.Euler(originRotation);
    }
}

using DG.Tweening;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{

    [Header("Rotation properties")]
    [SerializeField] private Transform head;
    [SerializeField] private float MouseYIntensivity;
    [SerializeField] private float MouseXIntensivity;
    [SerializeField] private float MinRotateX;
    [SerializeField] private float MaxRotateX;
    private Vector3 originBodyRotation;
    private Vector3 originHeadRotation;

    public void Init()
    {
        originBodyRotation = transform.localEulerAngles;
        originHeadRotation = head.localEulerAngles;
    }
    public void AfterInit()
    {

    }
    public void EveryFrame()
    {

    }
    public void AfterEveryFrame()
    {
        float rotationoY = Input.GetAxis("Mouse X") * MouseYIntensivity;
        float rotationX = -Input.GetAxis("Mouse Y") * MouseXIntensivity;

        originBodyRotation += new Vector3(0, rotationoY, 0);
        originHeadRotation += new Vector3(rotationX, 0, 0);

        originHeadRotation.x = Mathf.Clamp(originHeadRotation.x, MinRotateX, MaxRotateX);

        transform.rotation = Quaternion.Euler(originBodyRotation);
        head.rotation = Quaternion.Euler(originBodyRotation + originHeadRotation);
    }

    public void HeadShake(float ReboundDuration, float ReboundPositionForce, float ReboundRotationForce)
    {
        head.transform.DOShakePosition(ReboundDuration, new Vector3(0,0, ReboundPositionForce), 5, 45, false, true, ShakeRandomnessMode.Harmonic);
        head.transform.DOShakeRotation(ReboundDuration, new Vector3(ReboundRotationForce, 0, 0), 10, 60, true, ShakeRandomnessMode.Harmonic);
    }
}

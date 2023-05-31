using UnityEngine;

public class PlayerInteraction : PlayerWeapon
{
    [Header("Ray to check collide")]
    [SerializeField] private Transform originPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float length;
    [SerializeField] private LayerMask layerMask;

    [Header("Player Hand")]
    [SerializeField] private Transform hand;

    RaycastHit hit;
    public void Update()
    {
        CheckInteractibleObject();
    }

    private void CheckInteractibleObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(originPosition.position, originPosition.TransformDirection(direction), out hit, length, layerMask))
            {
                if(hit.transform.TryGetComponent(out Weapon weapon))
                {
                    if(this.weapon != null)
                    {
                        this.weapon.RemoveFromPlayerHand();
                    }
                    weapon.Interaction(hand.transform);
                    this.weapon = weapon;
                }
                Debug.Log(hit.transform.name);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(originPosition.position, originPosition.TransformDirection(direction) * length);
    }
}

using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : PlayerData
{
    [Header("Ray to check collide")]
    [SerializeField] private Transform originPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float length;
    [SerializeField] private LayerMask layerMask;

    [Header("Player Hand")]
    [SerializeField] private Transform hand;

    RaycastHit hit;
    public override void EveryFrame()
    {
        CheckInteractibleObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (weapon != null)
            {
                DropWeapon();
            }
            GetInteractibbleObject();
        }

        if (Input.GetMouseButton(0) && weapon != null)
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && weapon != null)
        {
            Recharge();
        }
    }

    private void Shoot()
    {
        weapon.Shoot(originPosition);
    }
    private void Recharge()
    {
        weapon.Recharge();
    }
    
    private void DropWeapon()
    {
        weapon.RemoveFromPlayerHand();
        weapon = null;
    }

    private void CheckInteractibleObject()
    {
        if (Physics.Raycast(originPosition.position, originPosition.TransformDirection(direction), out hit, length, layerMask))
        {
            if (hit.transform.TryGetComponent(out Interactable interactable))
            {
                EventManager.Instance.OnPlayerInteraction(true);
                Debug.Log(hit.collider + "HEllo");
            }
        }
        else
        {
            EventManager.Instance.OnPlayerInteraction(false);
        }
    }
    private void GetInteractibbleObject()
    {
        if (hit.collider != null && hit.transform.TryGetComponent(out Interactable interactable))
        {
            if (hit.transform.TryGetComponent(out Weapon weapon))
            {
                if (this.weapon != null)
                {
                    DropWeapon();
                }
                weapon.Interaction(hand.transform);
                this.weapon = weapon;
            }
            else
            {
                
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(originPosition.position, originPosition.TransformDirection(direction) * length);
    }
}

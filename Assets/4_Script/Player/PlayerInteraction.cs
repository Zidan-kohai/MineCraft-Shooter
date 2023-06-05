using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

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

    public override void Init()
    {
        CheckInteractibleObject();
    }
    public override void EveryFrame()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (weapon != null && hit.collider == null || weapon != null && !hit.transform.TryGetComponent(out Interactable interactable))
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
                EventManager.Instance.OnPlayerInteraction(interactable);
            }
        }
        else
        {
            EventManager.Instance.OnPlayerInteraction(null);
        }

        DOTween.Sequence()
            .AppendInterval(0.5f).OnComplete(() =>
            {
                CheckInteractibleObject();
            });
    }
    private void GetInteractibbleObject()
    {
        if (hit.collider != null && hit.transform.TryGetComponent(out Interactable interactable))
        {
            if (hit.transform.TryGetComponent(out Weapon weapon))
            {
                bool canBuy = weapon.BuyOrIsBuyed(GameManager.Instance.GetMoney());

                if (this.weapon != null && canBuy)
                {
                    DropWeapon();

                }
                if (canBuy)
                {
                    weapon.Interaction(hand.transform);
                    this.weapon = weapon;
                }
            }
            else if(hit.transform.TryGetComponent(out ShopPatrons shopPatrons) && this.weapon != null)
            {
                shopPatrons.Interaction(this.weapon);
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(originPosition.position, originPosition.TransformDirection(direction) * length);
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteraction : PlayerData
{
    [Header("Ray to check collide")]
    [SerializeField] private Transform originPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float length;

    [Header("Player Hand")]
    [SerializeField] private Transform hand;

    [Header("Conponents")]
    [SerializeField] private PlayerRotation playerRotation;


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
        WeaponState weaponState = weapon.Shoot(originPosition);

        if (weaponState == WeaponState.Shoot)
        {
            animator.SetTrigger("Shoot");
            playerRotation.HeadShake(weapon.reboundDuration, weapon.reboundPositionForce, weapon.reboundRotationForce);
        }
        else if(weaponState == WeaponState.Recharge)
        {
            animator.SetTrigger("Recharge");
        }
    }
    private void Recharge()
    {
        WeaponState weaponState = weapon.Recharge();

        if(weaponState == WeaponState.Recharge)
        {
            animator.SetTrigger("Recharge");
        }
    }
    
    private void DropWeapon()
    {
        weapon.RemoveFromPlayerHand();
        weapon = null;

        animator.SetTrigger("RemoveGun");
    }
    
    private void CheckInteractibleObject()
    {
        if (Physics.Raycast(originPosition.position, originPosition.TransformDirection(direction), out hit, length))
        {
            if (hit.transform.TryGetComponent(out Interactable interactable))
            {
                EventManager.Instance.OnPlayerInteraction(interactable);
            }
            else
            {
                EventManager.Instance.OnPlayerInteraction(null);
            }
        }
        else
        {
            EventManager.Instance.OnPlayerInteraction(null);
        }

        DOTween.Sequence()
            .AppendInterval(0.4f).OnComplete(() =>
            {
                CheckInteractibleObject();
            }).SetLink(gameObject);
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
                    WeaponTurnOn(weapon);
                }
            }
            else if(hit.transform.TryGetComponent(out ShopPatrons shopPatrons) && this.weapon != null)
            {
                shopPatrons.Interaction(this.weapon);
            }

        }
    }

    private void WeaponTurnOn(Weapon weapon)
    {
        if(weapon is Gun)
        {
            animator.SetTrigger("GetGun");
        }
        else if(weapon is MK)
        {
            animator.SetTrigger("GetMK");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(originPosition.position, originPosition.TransformDirection(direction) * length);
    }
}

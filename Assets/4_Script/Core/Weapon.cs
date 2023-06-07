using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : Interactable
{
    [Header("BuyProperties")]
    [SerializeField] protected bool isBuyed;

    [Header("Patrons Properties")]
    [SerializeField] protected int allPatrons;
    [SerializeField] protected int maxPatronsInMagazine;
    [SerializeField] protected int currentPatronsInMagazine;

    [Header("Transform in Player Hand")]
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private bool isInPlayerHand = false;
    [Header("Recharge Properties")]
    [SerializeField] protected float rechargeTime;
    [SerializeField] protected bool isRecharge;

    [Header("Shoot Properties")]
    [SerializeField] private float delayBetweenShoot;
    [SerializeField] protected bool canShoot = true;


    [Header("Rebound Properties")]
    [SerializeField] public float reboundDuration;
    [SerializeField] public float reboundPositionForce;
    [SerializeField] public float reboundRotationForce;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    public virtual void Idle() { }

    public virtual WeaponState Shoot(Transform OriginPosition)
    {
        if(!canShoot)
        {
            return WeaponState.Idle;
        }
        canShoot = false;
        if (currentPatronsInMagazine == 0)
        {
            if (allPatrons > 0)
            {
                Recharge();
                return WeaponState.Recharge;
            }
            else
            {
                return WeaponState.Idle;
            }
        }
        currentPatronsInMagazine--;

        DOTween.Sequence()
            .AppendInterval(delayBetweenShoot).OnComplete(() =>
            {
                canShoot = true;
            });
        EventManager.Instance.OnShoot(currentPatronsInMagazine, allPatrons, maxPatronsInMagazine);
        return WeaponState.Shoot;
    }

    public virtual void Inspect() { }

    public virtual WeaponState Recharge()
    {
        if (currentPatronsInMagazine == maxPatronsInMagazine || allPatrons == 0 || isRecharge)
        {
            Debug.Log("currentPatronsInMagazine equal maxPatronsInMagazine || allPatrons equal 0 || Recharging");
            return WeaponState.Idle;
        }

        isRecharge = true;
        canShoot = false;
        DOTween.Sequence()
            .AppendInterval(rechargeTime).OnComplete(() =>
            {
                isRecharge = false;
                canShoot = true;
                int countPatronsToFullMagazine = maxPatronsInMagazine - currentPatronsInMagazine;

                int minCountPatronsCanAdd = allPatrons < countPatronsToFullMagazine ? allPatrons : countPatronsToFullMagazine;
                
                int howManyPatronsWeCanAdd = Mathf.Clamp(allPatrons - countPatronsToFullMagazine, minCountPatronsCanAdd, countPatronsToFullMagazine);

                currentPatronsInMagazine += howManyPatronsWeCanAdd;
                allPatrons -= howManyPatronsWeCanAdd;

                if (isInPlayerHand)
                {
                    EventManager.Instance.OnShoot(currentPatronsInMagazine, allPatrons, maxPatronsInMagazine);
                }
            });
        return WeaponState.Recharge;
    }

    public override void Interaction(Transform parent)
    {
        AddToPlayerHand(parent);
        transform.localPosition = position;
        transform.localRotation = Quaternion.Euler(rotation);

        EventManager.Instance.OnShoot(currentPatronsInMagazine, allPatrons, maxPatronsInMagazine);
    }

    public void AddToPlayerHand(Transform parent)
    {
        transform.SetParent(parent);
        rb.isKinematic = true;
        transform.GetComponent<Collider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        isInPlayerHand = true;
    }
    public void RemoveFromPlayerHand()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        transform.GetComponent<Collider>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        isInPlayerHand = false;
        EventManager.Instance.OnShoot(0, 0, 0);
    }

    public bool GetIsBuyed()
    {
        return isBuyed;
    }

    public bool BuyOrIsBuyed(int money)
    {
        if (money >= cost && !isBuyed)
        {
            isBuyed = true;
            money -= cost;
            EventManager.Instance.OnSetMoney(money);
        }

        return isBuyed;
    }

    public void AddPatron(int addingPatron)
    {
        allPatrons += addingPatron;
        canShoot = true;
        EventManager.Instance.OnShoot(currentPatronsInMagazine, allPatrons, maxPatronsInMagazine);
    }
}
public enum WeaponState
{
    Idle,
    Shoot,
    Recharge
}
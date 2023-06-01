using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : Interactable
{

    [Header("Patrons Properties")]
    [SerializeField] protected int allPatrons;
    [SerializeField] protected int maxPatronsInMagazine;
    [SerializeField] protected int currentPatronsInMagazine;

    [Header("Transform in Player Hand")]
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;

    [Header("Recharge Properties")]
    [SerializeField] protected float rechargeTime;
    [SerializeField] protected bool isRecharge;

    [Header("Shoot Properties")]
    [SerializeField] private float delayBetweenShoot;
    [SerializeField] protected bool canShoot = true;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    public virtual void Idle() { }

    public virtual void Shoot(Transform OriginPosition)
    {
        if(!canShoot)
        {
            return;
        }
        canShoot = false;
        if (currentPatronsInMagazine == 0)
        {
            Recharge();
            return;
        }
        currentPatronsInMagazine--;

        DOTween.Sequence()
            .AppendInterval(delayBetweenShoot).OnComplete(() =>
            {
                canShoot = true;
            });

        EventManager.Instance.OnShoot(currentPatronsInMagazine,allPatrons,maxPatronsInMagazine);
    }

    public virtual void Inspect() { }

    public virtual void Recharge()
    {
        if (currentPatronsInMagazine == maxPatronsInMagazine || allPatrons == 0 || isRecharge)
        {
            Debug.Log("currentPatronsInMagazine equal maxPatronsInMagazine || allPatrons equal 0 || Recharging");
            return;
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
                EventManager.Instance.OnShoot(currentPatronsInMagazine, allPatrons, maxPatronsInMagazine);
            });
        
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
    }
    public void RemoveFromPlayerHand()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        transform.GetComponent<Collider>().enabled = true;

        EventManager.Instance.OnShoot(0, 0, 0);
    }

}

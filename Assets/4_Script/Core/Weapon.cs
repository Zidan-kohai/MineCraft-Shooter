using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour, Interactable
{

    [Header("Patrons Properties")]
    [SerializeField] protected int allPatrons;
    [SerializeField] protected int maxPatronsInMagazine;
    [SerializeField] protected int currentPatronsInMagazine;

    [Header("Transform in Player Hand")]
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;

    [Header("Recharge Properties")]
    [SerializeField] private float rechargeTime;
    [SerializeField] private float lastedTimeFromLstRecharge;
    [SerializeField] private bool isRecharge;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    public virtual void Idle() { }

    public virtual void Shoot()
    {
        if (currentPatronsInMagazine == 0)
        {
            Recharge();
            return;
        }
        currentPatronsInMagazine--;
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
        DOTween.Sequence()
            .AppendInterval(rechargeTime).OnComplete(() =>
            {
                isRecharge = false;
                int countPatronsToFullMagazine = maxPatronsInMagazine - currentPatronsInMagazine;
                int minCountPatronsCanAdd = allPatrons < maxPatronsInMagazine ? allPatrons : maxPatronsInMagazine;
                int howManyPatronsWeCanAdd = Mathf.Clamp(allPatrons - countPatronsToFullMagazine, minCountPatronsCanAdd, countPatronsToFullMagazine);

                currentPatronsInMagazine += howManyPatronsWeCanAdd;
                allPatrons -= howManyPatronsWeCanAdd;
            });

    }

    public void Interaction(Transform parent)
    {
        AddToPlayerHand(parent);
        transform.localPosition = position;
        transform.localRotation = Quaternion.Euler(rotation);

        Debug.Log("Interaction");
    }

    public void AddToPlayerHand(Transform parent)
    {
        transform.SetParent(parent);
        rb.isKinematic = true;
    }
    public void RemoveFromPlayerHand()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
    }

}

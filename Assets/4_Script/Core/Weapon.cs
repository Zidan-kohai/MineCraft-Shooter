using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public abstract class Weapon : MonoBehaviour, Interactable
{
    [Header("Patrons Properties")]
    [SerializeField] protected int allPatrons;
    [SerializeField] protected int maxPatronsInMagazine;
    protected int currentPatronsInMagazine;

    [Header("Transform in Player Hand")]
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    public virtual void Idle() { }

    public virtual void Shoot() 
    { 
        if(currentPatronsInMagazine == 0)
        {
            Recharge();
            return;
        }
        Debug.Log("Shoot");
    }

    public virtual void Inspect() { }

    public virtual void Recharge()
    {
        if (currentPatronsInMagazine == maxPatronsInMagazine || allPatrons == 0) return;
        
        int countPatronsToFullMagazine = maxPatronsInMagazine - currentPatronsInMagazine;
        int howManyPatronsWeCanAdd = Mathf.Clamp(allPatrons - countPatronsToFullMagazine, 1, countPatronsToFullMagazine);

        currentPatronsInMagazine += howManyPatronsWeCanAdd;
        allPatrons -= howManyPatronsWeCanAdd;
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

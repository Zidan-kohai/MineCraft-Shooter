using JetBrains.Annotations;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Buying Properties")] 
    [SerializeField] protected bool isBuyed;
    [SerializeField] protected int cost;
    public virtual void Interaction(Transform parent) { }

    public bool GetIsBuyed()
    {
        return isBuyed;
    }

    public bool BuyOrIsBuyed(int money)
    {
        if(money >= cost && !isBuyed)
        {
            isBuyed = true;
            money -= cost;
            EventManager.Instance.OnSetMoney(money);
        }

        return isBuyed;
    }

    public int GetCost()
    {
        return cost;
    }

    public void SetCost(int value)
    {
        cost = value;
    }
}

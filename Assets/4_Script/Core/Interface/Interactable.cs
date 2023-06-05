using JetBrains.Annotations;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Buying Properties")] 
    [SerializeField] protected int cost;
    public virtual void Interaction(Transform parent) { }
    public virtual void Interaction() { }
    public int GetCost()
    {
        return cost;
    }

    public void SetCost(int value)
    {
        cost = value;
    }
}

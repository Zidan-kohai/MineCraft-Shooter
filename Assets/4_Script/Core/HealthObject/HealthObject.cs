using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class HealthObject : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;

    public virtual void Init()
    {

    }
    public virtual void Init(List<Transform> positions)
    {

    }
    public virtual void AfterInit()
    {

    }
    public virtual void EveryFrame()
    {

    }
    public virtual void AfterEveryFrame()
    {

    }

    public virtual void Death() {  }
    public virtual void GetDamage(int damage) { }
}

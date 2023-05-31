using UnityEngine;

public abstract class HealthObject : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    public virtual void Death() {  }
    public virtual void GetDamage(int damage) { }
}

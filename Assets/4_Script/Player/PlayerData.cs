using DG.Tweening;
using UnityEngine;

public class PlayerData : HealthObject
{
    [Header("Weapons")]
    [SerializeField] protected Weapon weapon;

    [Header("Animator")]
    [SerializeField] protected Animator animator;
    public override void GetDamage(int damage, Vector3 direction)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if(health == 0)
        {
            Death();
        }
        transform.DOMove(transform.position + (new Vector3(direction.x, 1, direction.z) * 1.3f), 0.3f);
    }

    public override void Death()
    {
        EventManager.Instance.OnDeath(this);
        Debug.Log("Player Death");
    }
}

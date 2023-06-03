using UnityEngine;

public class PlayerData : HealthObject
{
    [Header("Weapons")]
    [SerializeField] protected Weapon weapon;
    public override void GetDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if(health == 0)
        {
            Death();
        }
    }

    public override void Death()
    {
        EventManager.Instance.OnDeath(this);
        Debug.Log("Player Death");
    }
}

using UnityEngine;

public class PlayerData : HealthObject
{
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
        Debug.Log("Player Death");
    }
}
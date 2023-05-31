using UnityEngine;

public class Enemy : HealthObject
{
    public override void GetDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health == 0)
        {
            Death();
        }
        Debug.Log("Enemy Getting Damage");
    }

    public override void Death()
    {
        Debug.Log("Enemy Death");
    }
}

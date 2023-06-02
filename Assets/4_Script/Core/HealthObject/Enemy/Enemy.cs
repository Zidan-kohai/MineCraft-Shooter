using UnityEngine;
using UnityEngine.AI;

public class Enemy : HealthObject
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    public override void Init()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void EveryFrame()
    {
        Walk();
    }
    private void Walk()
    {
        if (target == null)
        {
            target = GameManager.Instance.GetNextVillegerForZombi(transform.position);
            
        }

        agent.destination = target.position;
    }

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
        EventManager.Instance.OnDeath(this);
        Destroy(gameObject);
        Debug.Log("Enemy Death");
    }
}

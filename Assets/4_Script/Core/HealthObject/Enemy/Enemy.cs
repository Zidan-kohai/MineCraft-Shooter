using UnityEngine;
using UnityEngine.AI;

public class Enemy : HealthObject
{
    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Target")]
    [SerializeField] private HealthObject target;
    [SerializeField] private float distanceToTarget;

    
    [SerializeField] private int damage;
    [SerializeField] private float distanceToAttack;
    [SerializeField] private float attackDelay;
    [SerializeField] private float lastedTimeFromLastAttack;
    public override void Init()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void EveryFrame()
    {
        Walk();
        if(distanceToTarget < distanceToAttack && lastedTimeFromLastAttack > attackDelay)
        {
            Attack();
        }
        lastedTimeFromLastAttack += Time.deltaTime;
    }
    private void Walk()
    {
        if (target == null)
        {
            target = GameManager.Instance.GetNextVillegerForZombi(transform.position);
            
        }

        agent.destination = target.transform.position;
        distanceToTarget = (transform.position - target.transform.position).magnitude;
    }

    private void Attack()
    {
        target.GetDamage(damage);
        lastedTimeFromLastAttack = 0;
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

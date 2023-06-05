using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : HealthObject
{
    [Header("Components")]
    [SerializeField] protected NavMeshAgent agent;

    [Header("Target")]
    [SerializeField] protected HealthObject target;
    [SerializeField] protected float distanceToTarget;

    [Header("Attack Properties")]
    [SerializeField] protected int damage;
    [SerializeField] protected float distanceToAttack;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float lastedTimeFromLastAttack;

    [Header("Enemy Properties")]
    [SerializeField] private int cost;
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

    protected virtual void Attack()
    {
        target.GetDamage(damage, (target.transform.position - transform.position).normalized);
        lastedTimeFromLastAttack = 0;
    }

    public override void GetDamage(int damage, Vector3 direction)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health == 0)
        {
            Death();
        }

        transform.DOMove(transform.position + (new Vector3(direction.x, 1, direction.z) * 1.3f), 0.3f);
    }

    public override void Death()
    {
        EventManager.Instance.OnDeath(this);
        EventManager.Instance.OnSetMoney(GameManager.Instance.GetMoney() + cost);
        Destroy(gameObject);
    }

    public int GetCost()
    {
        return cost;
    }
}

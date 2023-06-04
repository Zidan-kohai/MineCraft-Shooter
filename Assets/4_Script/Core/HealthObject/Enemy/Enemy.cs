using DG.Tweening;
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
        Debug.Log("Enemy Getting Damage");

        transform.DOMove(transform.position + (new Vector3(direction.x, 1, direction.z) * 1.3f), 0.3f);
    }

    public override void Death()
    {
        EventManager.Instance.OnDeath(this);
        Destroy(gameObject);
        Debug.Log("Enemy Death");
    }
}

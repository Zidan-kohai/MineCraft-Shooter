using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villeger : HealthObject
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToChooseNextTarget;
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
        if (target == null || Mathf.Abs((target.position - transform.position).magnitude) < distanceToChooseNextTarget)
        {
            target = GameManager.Instance.GetNextPositionForVilleger();
            agent.destination = target.position;
        }
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

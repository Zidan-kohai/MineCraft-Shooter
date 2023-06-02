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
        Debug.Log("Enemy Death");
    }
}

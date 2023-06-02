using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villeger : HealthObject
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<Transform> transforms;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToChooseNextTarget;
    public override void Init(List<Transform> positions)
    {
        agent = GetComponent<NavMeshAgent>();
        transforms = positions;
    }
    public override void EveryFrame()
    {
        Walk();
    }
    private void Walk()
    {
        if (target == null || Mathf.Abs((target.position - transform.position).magnitude) < distanceToChooseNextTarget)
        {
            target = transforms[Random.Range(0, transforms.Count)];
            agent.destination = target.position;
        }
    }
}

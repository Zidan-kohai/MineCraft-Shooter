using UnityEngine;

public class SimpleEnemy : Enemy
{
    [Header("Components")]
    [SerializeField] protected Animator animator;

    public override void EveryFrame()
    {
        base.EveryFrame();

        animator.SetFloat("distance", distanceToTarget);
    }

    protected override void Attack()
    {
        base.Attack();

        animator.SetTrigger("attack");
    }
}

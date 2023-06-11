using UnityEngine;

public class ArcherEnemy : Enemy
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
        lastedTimeFromLastAttack = 0;
    }
}

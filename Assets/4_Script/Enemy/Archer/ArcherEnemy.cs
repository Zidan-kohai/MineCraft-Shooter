using UnityEngine;

public class ArcherEnemy : Enemy
{
    [Header("Components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Bullet arrow;
    [SerializeField] protected Transform arrowPosition;
    [SerializeField] protected float arrowSpeed;
    public override void EveryFrame()
    {
        base.EveryFrame();

        animator.SetFloat("distance", distanceToTarget);
    }
    protected override void Attack()
    {
        animator.SetTrigger("attack");
        lastedTimeFromLastAttack = 0;
    }

    private void Shoot()
    {
        transform.forward = (target.transform.position - transform.position).normalized;
        Bullet bullet = Instantiate(arrow, arrowPosition.position, Quaternion.identity, arrowPosition);
        bullet.transform.SetParent(null);
        bullet.Init(transform.forward, damage, arrowSpeed);
    }
}

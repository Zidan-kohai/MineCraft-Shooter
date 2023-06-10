
using DG.Tweening;
using UnityEngine;

public class Creeper : Enemy
{
    [SerializeField] private bool isAttacked;


    [Header("Explosion")]
    [SerializeField] private Vector3 scaleCurrentExplosion;
    [SerializeField] private float scaleDuration;
    [SerializeField] private float sphereRadius;

    [Header("Components")]
    [SerializeField] protected Animator animator;
    public override void EveryFrame()
    {
        base.Walk();
        animator.SetFloat("distance", distanceToTarget);

        if (distanceToTarget < distanceToAttack && !isAttacked)
        {
            Attack();
        }
        lastedTimeFromLastAttack += Time.deltaTime;
    }

    protected override void Attack()
    {
        isAttacked = true;
        audioSource.clip = diethSound;
        audioSource.Play();

        animator.SetTrigger("attack");

        DOTween.Sequence()
            .Append(transform.DOScale(scaleCurrentExplosion, scaleDuration)).SetEase(Ease.Linear)
            .Append(transform.DOScale(1f, scaleDuration)).SetEase(Ease.Linear)
            .SetLoops(4)
            .OnComplete(() =>
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius);
                foreach(Collider collider in colliders)
                {
                    if(collider.gameObject.TryGetComponent(out HealthObject healthObject))
                    {
                        healthObject.GetDamage(damage, (healthObject.transform.position - transform.position).normalized);
                    }
                }
                base.Death();
            }).SetLink(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}

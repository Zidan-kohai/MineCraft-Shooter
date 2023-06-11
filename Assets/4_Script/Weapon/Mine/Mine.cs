using DG.Tweening;
using System.IO.Compression;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float timeToExplosion;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private float sphereRadius;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    private bool canExplosion;
    private bool exploded;

    private void Update()
    {
        DOTween.Sequence()
            .AppendInterval(1f)
            .OnStepComplete(() =>
            {
                if (!canExplosion)
                {
                    CheckCollider();
                }
                if (canExplosion && !exploded)
                {
                    audioSource.clip = clip;
                    audioSource.Play();

                    DOTween.Sequence().
                    AppendInterval(timeToExplosion).OnComplete(() =>
                    {

                        Explosion();
                    }).SetLink(gameObject);

                    exploded = true;
                }
            }).SetLoops(-1).SetLink(gameObject);
    }
    private void CheckCollider()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                canExplosion = true;
                return;
            }
        }
    }
    private void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out HealthObject healthObject))
            {
                healthObject.GetDamage(damage, (healthObject.transform.position - transform.position).normalized, damage / 7);
            }
        }
        ParticleSystem particle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        DOTween.Sequence()
            .AppendInterval(0.4f).OnComplete(() =>
            {
                Destroy(particle.gameObject);
            }).SetLink(particle.gameObject);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}

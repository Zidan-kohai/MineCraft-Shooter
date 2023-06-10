using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Granade : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float sphereRadius;
    [SerializeField] private float timeToExplosion;
    [SerializeField] private ParticleSystem ExplosionParticle;

    private Sequence sequence;
    public void Init(Vector3 direction)
    {
        rb.velocity = direction * speed;

        sequence = DOTween.Sequence()
            .AppendInterval(timeToExplosion)
            .OnComplete(() =>
            {
                Explosion();

            }).SetLink(gameObject);
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
        ParticleSystem particle = Instantiate(ExplosionParticle, transform.position, Quaternion.identity);
        DOTween.Sequence()
            .AppendInterval(0.4f).OnComplete(() =>
            {
                Destroy(particle.gameObject);
            }).SetLink(particle.gameObject);
        
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        sequence.Kill();

        Explosion();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}

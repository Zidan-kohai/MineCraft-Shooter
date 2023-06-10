using DG.Tweening;
using UnityEngine;


[RequireComponent (typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int damage;
    [SerializeField] private ParticleSystem getDamageParticle;
    public void Init(Vector3 direction, int damage, float speed)
    {
        this.damage = damage;

        rb.velocity = direction * speed;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Enemy enemy))
        {   
            enemy.GetDamage(damage, (collision.transform.position - transform.position).normalized, damage / 7);
            ParticleSystem particle = Instantiate(getDamageParticle, transform.position, Quaternion.identity);

            DOTween.Sequence()
                .AppendInterval(1).OnComplete(() =>
                {
                    Destroy(particle.gameObject);
                }).SetLink(particle.gameObject);
        }

        Destroy(gameObject);
    }
}

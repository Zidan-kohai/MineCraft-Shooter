using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class Enemy : HealthObject
{
    [Header("Components")]
    [SerializeField] protected NavMeshAgent agent;

    [Header("Target")]
    [SerializeField] protected HealthObject target;
    [SerializeField] protected float distanceToTarget;

    [Header("Attack Properties")]
    [SerializeField] protected int damage;
    [SerializeField] protected float distanceToAttack;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float lastedTimeFromLastAttack;

    [Header("Shop Properties")]
    [SerializeField] private int cost;

    [Header("Sound Properties")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip roarSound;
    [SerializeField] protected AudioClip diethSound;
    [SerializeField] protected AudioClip getDamageSound;
    [SerializeField] private float minTimeToRoar;
    [SerializeField] private float maxTimeToRoar;
    [SerializeField] private float chooseTimeToRoar;
    public override void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        Roar();
    }

    public override void EveryFrame()
    {
        DOTween.Sequence()
            .AppendInterval(1).OnComplete(() =>
            {
                Walk();
            });
        if(distanceToTarget < distanceToAttack && lastedTimeFromLastAttack > attackDelay)
        {
            Attack();
        }
        lastedTimeFromLastAttack += Time.deltaTime;
    }

    protected void Walk()
    {
        if (target == null)
        {
            target = GameManager.Instance.GetNextVillegerForZombi(transform.position);
            
        }

        agent.destination = target.transform.position;
        distanceToTarget = (transform.position - target.transform.position).magnitude;
    }

    private void Roar()
    {
        audioSource.clip = roarSound;
        audioSource.Play();
        chooseTimeToRoar = Random.Range(minTimeToRoar, maxTimeToRoar);

        DOTween.Sequence()
            .AppendInterval(chooseTimeToRoar).OnComplete(() =>
            {
                Roar();
            }).SetLink(gameObject);
    }
    protected virtual void Attack()
    {
        target.GetDamage(damage, (target.transform.position - transform.position).normalized, damage / 7);
        lastedTimeFromLastAttack = 0;
    }

    public override void GetDamage(int damage, Vector3 direction, float force)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health == 0)
        {
            Death();
        }
        audioSource.clip = getDamageSound;
        audioSource.Play();

        transform.DOMove(transform.position + new Vector3(direction.x * force, 1, direction.z * force), 0.3f);
    }

    public override void Death()
    {
        EventManager.Instance.OnDeath(this);
        EventManager.Instance.OnSetMoney(GameManager.Instance.GetMoney() + cost);
        Destroy(gameObject);
    }

    public int GetCost()
    {
        return cost;
    }
}

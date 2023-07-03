using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class Villeger : HealthObject
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToChooseNextTarget;
    [SerializeField] private Animator animator;


    [Header("Sound Properties")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip roarSound;
    [SerializeField] private AudioClip diethSound;
    [SerializeField] private AudioClip getDamageSound;
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
        Walk();
    }
    private void Walk()
    {
        if (target == null || Mathf.Abs((target.position - transform.position).magnitude) < distanceToChooseNextTarget)
        {
            target = GameManager.Instance.GetNextPositionForVilleger();
            agent.destination = target.position;
            animator.SetTrigger("Walk");
        }
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
        Destroy(gameObject);
        Debug.Log("Villeger Death");
    }
}

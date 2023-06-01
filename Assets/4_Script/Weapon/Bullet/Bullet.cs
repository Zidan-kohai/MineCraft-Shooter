using UnityEngine;


[RequireComponent (typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    public void Init(Vector3 direction, int damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;

        rb.velocity = direction * speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Enemy enemy))
        {   
            enemy.GetDamage(damage);
        }
        //Destroy(gameObject);
    }
}
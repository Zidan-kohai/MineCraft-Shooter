using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Bullet properties")]
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private int damage;
    [SerializeField] private float minSpread;
    [SerializeField] private float maxSpread;
    [SerializeField] private float distance;
    [SerializeField] private ParticleSystem particleDamage;
    [SerializeField] LayerMask layerMask;
    public override WeaponState Shoot(Transform originPosition)
    {
        bool canShoot = currentPatronsInMagazine > 0 && this.canShoot && !isRecharge;

        if (canShoot)
        {
            bulletsSpawn(originPosition);
        }


        return base.Shoot(originPosition);

    }

    public override WeaponState Recharge()
    {
        return base.Recharge();
    }

    private void bulletsSpawn(Transform originPosition)
    {

        for (int i = 0; i < 10; i++)
        {
            bulletSpawnPosition.rotation = Quaternion.identity;

            float currentSpread = Random.Range(minSpread, maxSpread);

            bulletSpawnPosition.localRotation = Quaternion.Euler(bulletSpawnPosition.localRotation.x + currentSpread,
                bulletSpawnPosition.localRotation.y + currentSpread, bulletSpawnPosition.localRotation.z + currentSpread);


            RaycastHit hit;
            if (Physics.Raycast(bulletSpawnPosition.position, -bulletSpawnPosition.right, out hit, distance, layerMask))
            {
                if(hit.transform.TryGetComponent(out Enemy enemy))
                {
                    enemy.GetDamage((int)(damage / hit.distance), (enemy.transform.position - transform.position).normalized, (damage / hit.distance));


                    ParticleSystem particles = Instantiate(particleDamage, hit.point, Quaternion.identity);
                    DOTween.Sequence()
                        .AppendInterval(0.4f)
                        .OnComplete(() =>
                        {
                            Destroy(particles.gameObject);
                        }).SetLink(gameObject);

                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(bulletSpawnPosition.position, -bulletSpawnPosition.right * distance);
    }
}

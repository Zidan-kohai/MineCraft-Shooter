using UnityEngine;

public class Gun : Weapon
{
    [Header("Bullet properties")]
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    public override void Idle() { }

    public override void Shoot()
    {
        if(currentPatronsInMagazine > 0 && canShoot && !isRecharge)
        {
            bulletSpawn();
        }
        base.Shoot();
        
    }

    public override void Inspect() { }

    public override void Recharge()
    {
        base.Recharge();
    }

    private void bulletSpawn()
    {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity, bulletSpawnPosition);
        bullet.transform.SetParent(null);
        bullet.Init(bulletSpawnPosition.forward, damage, bulletSpeed);
        
    }
}

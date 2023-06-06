using UnityEngine;

public class Gun : Weapon
{
    [Header("Bullet properties")]
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    public override void Idle() { }

    public override bool Shoot(Transform originPosition)
    {
        bool canShoot = currentPatronsInMagazine > 0 && this.canShoot && !isRecharge;
        if (canShoot)
        {
            bulletSpawn(originPosition);
        }
        base.Shoot(originPosition);

        return canShoot;
        
    }

    public override void Inspect() { }

    public override void Recharge()
    {
        base.Recharge();
    }

    private void bulletSpawn(Transform originPosition)
    {
        Ray ray = new Ray(originPosition.position, originPosition.forward);

        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 direction = targetPoint - bulletSpawnPosition.position;

        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity, bulletSpawnPosition);
        bullet.transform.SetParent(null);
        bullet.Init(direction.normalized, damage, bulletSpeed);
    }
}

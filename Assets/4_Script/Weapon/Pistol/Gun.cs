using UnityEngine;

public class Gun : Weapon
{
    public override void Idle() { }

    public override void Shoot()
    {
        base.Shoot();
        
    }

    public override void Inspect() { }

    public override void Recharge()
    {
        base.Recharge();
        
    }
}

using UnityEngine;

public class ArcherEnemy : Enemy
{
    protected override void Attack()
    {
        Debug.Log("Archer");
        lastedTimeFromLastAttack = 0;
    }
}

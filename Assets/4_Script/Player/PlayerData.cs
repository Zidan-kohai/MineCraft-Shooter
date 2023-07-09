using DG.Tweening;
using UnityEngine;

public class PlayerData : HealthObject
{
    [Header("Weapons")]
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected Granade granade;
    [SerializeField] protected int granadeCount;
    [SerializeField] protected Mine mine;
    [SerializeField] protected int mineCount;
    [SerializeField] protected LayerMask layerToPutMine;

    [Header("Animator")]
    [SerializeField] protected Animator animator;
    public override void GetDamage(int damage, Vector3 direction, float force)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if(health == 0)
        {
            Death();
        }
        transform.DOMove(transform.position + new Vector3(direction.x * force, 1, direction.z * force), 0.3f);
    }
    public void addGrenade(int count)
    {
        granadeCount += count;
        EventManager.Instance.OnUseBlowUp(granadeCount, mineCount);

        DataManager.Instance.SetGranadeCount(granadeCount);
    }
    public void addMine(int count)
    {
        mineCount += count;
        EventManager.Instance.OnUseBlowUp(granadeCount, mineCount);

        DataManager.Instance.SetMineInPlayerHand(mineCount);
    }

    public Weapon getWeapon() { return weapon; }

    public override void Death()
    {
        EventManager.Instance.OnDeath(this);
        Debug.Log("Player Death");
    }


    public void SetBlowUp(int granadeCount, int mineCount)
    {
        this.granadeCount = granadeCount;
        this.mineCount = mineCount;

        EventManager.Instance.OnUseBlowUp(granadeCount, mineCount);

        DataManager.Instance.SetMineInPlayerHand(mineCount);
        DataManager.Instance.SetGranadeCount(granadeCount);
    }
}

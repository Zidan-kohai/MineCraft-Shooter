using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainWindowManager : Window
{
    [Header("Bullets")]
    [SerializeField] private GameObject countPatronShowPanel;
    [SerializeField] private TMP_Text allPatrons;
    [SerializeField] private TMP_Text currentPatronsInMagazine;
    [SerializeField] private TMP_Text maxPatronsInMagazine;

    [Header("Interaction")]
    [SerializeField] private Image target;
    [SerializeField] private Sprite shootImage;
    [SerializeField] private Sprite useImage;

    [Header("Health Objects")]
    [SerializeField] private TMP_Text VillegerCount;
    [SerializeField] private TMP_Text EnemyCount;

    public override void Init()
    {
        EventManager.Instance.SubscribeOnShoot(OnShoot);
        EventManager.Instance.SubscribePlayerInteraction(SetTarget);
        EventManager.Instance.SubscribeOnDeath(SetHealthObject);

        EnemyCount.text = GameManager.Instance.GetEnemyCount().ToString();
        VillegerCount.text = GameManager.Instance.GetVillegerCount().ToString();
    }
    private void OnShoot(int currentPatronsInMagazine, int allPatrons, int maxPatronsInMagazine)
    {
        this.allPatrons.text =  allPatrons.ToString();
        this.currentPatronsInMagazine.text = currentPatronsInMagazine.ToString();
        this.maxPatronsInMagazine.text = maxPatronsInMagazine.ToString();
    }

    private void SetTarget(bool isInteraction)
    {
        if (isInteraction)
        {
            target.sprite = useImage;
        }
        else
        {
            target.sprite = shootImage;
        }
    }

    private void SetHealthObject(HealthObject healthObject)
    {
        if(healthObject is Enemy)
        {
            EnemyCount.text = GameManager.Instance.GetEnemyCount().ToString();
        }
        else if(healthObject is Villeger)
        {
             VillegerCount.text = GameManager.Instance.GetVillegerCount().ToString();
        }
    }

    public override void Destroy()
    {
        EventManager.Instance.UnsubscribeOnShoot(OnShoot);
        EventManager.Instance.UnsubscribePlayerInteraction(SetTarget);
        EventManager.Instance.UnsubscribeOnDeath(SetHealthObject);
    }
}

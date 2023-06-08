using DG.Tweening;
using System;
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
    [SerializeField] private TMP_Text AllMoney;
    [SerializeField] private GameObject buyUI;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text timeToNextWave;

    [Header("Health Objects")]
    [SerializeField] private TMP_Text VillegerCount;
    [SerializeField] private TMP_Text EnemyCount;

    [Header("Game Menu")]
    [SerializeField] private GameObject MainGameWindow;
    [SerializeField] private GameObject PauseWindow;
    [SerializeField] private GameObject OptionsWindow;
    [SerializeField] private GameObject WinWindow;
    [SerializeField] private GameObject LoseWindow;

    [Header("Main Menu")]
    [SerializeField] private GameObject MainMenuWindow;

    public override void Init()
    {
        EventManager.Instance.SubscribeOnShoot(OnShoot);
        EventManager.Instance.SubscribePlayerInteraction(SetTarget);
        EventManager.Instance.SubscribeOnDeath(SetHealthObjectCount);
        EventManager.Instance.SubscribeOnStopGame(PauseGame);
        EventManager.Instance.SubscribeOnLoseGame(LoseGame);
        EventManager.Instance.SubscribeOnSetMoney(SetMoney);
        EventManager.Instance.SubscribeOnNewWave(NewWave);
        EventManager.Instance.SubscribeOnEndWave(EndWave);


        GameManager.Instance.SetMoney(Convert.ToInt32(AllMoney.text));
    }

    public void PauseGame()
    {
        PauseWindow.SetActive(true);
        MainGameWindow.SetActive(false);
    }
    public void LoseGame()
    {
        LoseWindow.SetActive(true);
        MainGameWindow.SetActive(false);
    }

    private void SetMoney(int money)
    {
        AllMoney.text = money.ToString();
    }

    private void OnShoot(int currentPatronsInMagazine, int allPatrons, int maxPatronsInMagazine)
    {
        this.allPatrons.text =  allPatrons.ToString();
        this.currentPatronsInMagazine.text = currentPatronsInMagazine.ToString();
        this.maxPatronsInMagazine.text = maxPatronsInMagazine.ToString();
    }

    private void SetTarget(Interactable interaction)
    {
        if (interaction == null)
        {
            target.sprite = shootImage;
            if (buyUI.activeSelf)
            {
                buyUI.SetActive(false);
            }
        }
        else if (interaction)
        {
            target.sprite = useImage;

            if (interaction is Weapon && !((Weapon)interaction).GetIsBuyed())
            {
                buyUI.SetActive(true);
                cost.text = interaction.GetCost().ToString();
            }
            else if (interaction is ShopPatrons)
            {
                buyUI.SetActive(true);
                cost.text = interaction.GetCost().ToString();
            }
        }
    }

    private void SetHealthObjectCount(HealthObject healthObject)
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

    private void NewWave()
    {
        EnemyCount.text = GameManager.Instance.GetEnemyCount().ToString();
        VillegerCount.text = GameManager.Instance.GetVillegerCount().ToString();
    }

    private void EndWave(int timeToNextWave)
    {
        this.timeToNextWave.text = timeToNextWave.ToString();
        Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1).OnStepComplete(() =>
            {
                int newTime = (Convert.ToInt32(this.timeToNextWave.text) - 1);
                this.timeToNextWave.text = newTime.ToString();

                if(newTime <= 0)
                {
                    EventManager.Instance.OnNewWave();
                    sequence.Kill();
                }
            }).SetLoops(-1).SetLink(gameObject);
    }

    public override void Destroy()
    {
        EventManager.Instance.UnsubscribeOnShoot(OnShoot);
        EventManager.Instance.UnsubscribePlayerInteraction(SetTarget);
        EventManager.Instance.UnsubscribeOnDeath(SetHealthObjectCount);
        EventManager.Instance.UnsubscribeOnStopGame(PauseGame);
        EventManager.Instance.UnsubscribeOnLoseGame(LoseGame);
        EventManager.Instance.UnsubscribeOnSetMoney(SetMoney);
        EventManager.Instance.UnsubscribeOnNewWave(NewWave);
        EventManager.Instance.UnsubscribeOnEndWave(EndWave);
    }
}

using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainWindowManager : Window
{
    [Header("Bullets")]
    [SerializeField] private GameObject countPatronShowPanel;
    [SerializeField] private TMP_Text allPatrons;
    [SerializeField] private TMP_Text currentPatronsInMagazine;
    [SerializeField] private TMP_Text maxPatronsInMagazine;
    [SerializeField] private TMP_Text currentGrenade;
    [SerializeField] private TMP_Text currentMine;
    [SerializeField] private Image weaponImage;
    [SerializeField] private List<Sprite> weaponSprites;

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
    [SerializeField] private TMP_Text SaveVillegerText;

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
        EventManager.Instance.SubscribeOnUseBlowUp(OnUseBlowUpThing);
        EventManager.Instance.SubscribeOnStart(StartGame);

    }


    private void StartGame()
    {
        EnemyCount.text = GameManager.Instance.GetEnemyCount().ToString();
        VillegerCount.text = GameManager.Instance.GetVillegerCount().ToString();

        SaveVillegerText.gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(SaveVillegerText.transform.DOScale(1.3f, 0.3f))
            .Append(SaveVillegerText.transform.DOScale(1f, 0.3f))
            .SetLoops(5).OnComplete(() =>
            {
                SaveVillegerText.gameObject.SetActive(false);
            });
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

    private void OnShoot(int currentPatronsInMagazine, int allPatrons, int maxPatronsInMagazine, Weapon weapon)
    {
        weaponImage.color = new Color(1, 1, 1, 1);
        countPatronShowPanel.gameObject.SetActive(true);
        switch (weapon)
        {
            case Gun:
                weaponImage.sprite = weaponSprites[0];
                weaponImage.rectTransform.sizeDelta = new Vector2(300, 300);
                break;

            case MK:
                weaponImage.sprite = weaponSprites[1];
                weaponImage.rectTransform.sizeDelta = new Vector2(300, 200);
                break;

            case Shotgun:
                weaponImage.sprite = weaponSprites[2];
                weaponImage.rectTransform.sizeDelta = new Vector2(300, 200);
                break;

            case Weapon:
                countPatronShowPanel.gameObject.SetActive(false);
                this.allPatrons.text = allPatrons.ToString();
                this.currentPatronsInMagazine.text = currentPatronsInMagazine.ToString();
                this.maxPatronsInMagazine.text = maxPatronsInMagazine.ToString();
                weaponImage.color = new Color(1, 1, 1, 0);
                return;
        }

        this.allPatrons.text =  allPatrons.ToString();
        this.currentPatronsInMagazine.text = currentPatronsInMagazine.ToString();
        this.maxPatronsInMagazine.text = maxPatronsInMagazine.ToString();
    }

    private void OnUseBlowUpThing(int grenadeCount, int mineCount)
    {
        currentGrenade.text = grenadeCount.ToString();
        currentMine.text = mineCount.ToString();
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
            else if (interaction is Shop)
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
        EventManager.Instance.UnsubscribeOnUseBlowUp(OnUseBlowUpThing);
        EventManager.Instance.UnsubscribeOnStart(StartGame);
    }
}

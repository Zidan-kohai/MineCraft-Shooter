using System;
using System.Linq;

public class EventManager : Manager
{
    public static EventManager Instance;
    public override void Init()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private Action start;
    private Action<int, int,int, Weapon> shoot;
    private Action<Interactable> playerInteraction;
    private Action<HealthObject> death;
    private Action<int> setMoney;
    private Action stopGame;
    private Action loseGame;
    private Action newWave;
    private Action<int> endWave;
    private Action<int, int> useBlowUp;
    private Action showADV;

    #region Start
    public void SubscribeOnStart(Action startAction)
    {
        if (start == null)
        {
            start = startAction;
        }
        else if (!start.GetInvocationList().Contains(startAction))
        {
            start += startAction;
        }
    }

    public void UnsubscribeOnStart(Action startAction)
    {
        start -= startAction;
    }

    public void OnStart()
    {
        start?.Invoke();
    }
    #endregion

    #region Shoot
    public void SubscribeOnShoot(Action<int,int,int, Weapon> shootAction)
    {
        if(shoot == null)
        {
            shoot = shootAction;

        }else if (!shoot.GetInvocationList().Contains(shootAction))
        {
            shoot += shootAction;
        }
    }

    public void UnsubscribeOnShoot(Action<int,int,int, Weapon> shootAction)
    {
        shoot -= shootAction;
    }

    public void OnShoot(int currentPatronsInMagazine, int allPatrons, int maxPatronsInMagazine, Weapon weapon)
    {
        shoot?.Invoke(currentPatronsInMagazine, allPatrons, maxPatronsInMagazine, weapon);
    }

    #endregion

    #region PlayerInteraction
    public void SubscribePlayerInteraction(Action<Interactable> playerInteractionAction)
    {
        if (playerInteraction == null)
        {
            playerInteraction = playerInteractionAction;
        }
        else if (!playerInteraction.GetInvocationList().Contains(playerInteractionAction))
        {
            playerInteraction += playerInteractionAction;
        }
    }

    public void UnsubscribePlayerInteraction(Action<Interactable> playerInteractionAction)
    {
        playerInteraction -= playerInteractionAction;
    }

    public void OnPlayerInteraction(Interactable interactable)
    {
        playerInteraction?.Invoke(interactable);
    }

    #endregion

    #region Death
    public void SubscribeOnDeath(Action<HealthObject> deathAction)
    {
        if (death == null)
        {
            death = deathAction;
        }
        else if (!death.GetInvocationList().Contains(deathAction))
        {
            death += deathAction;
        }
    }

    public void UnsubscribeOnDeath(Action<HealthObject> deathAction)
    {
        death -= deathAction;
    }

    public void OnDeath(HealthObject obj)
    {
        death?.Invoke(obj);
    }
    #endregion

    #region SetMoney
    public void SubscribeOnSetMoney(Action<int> setMoneyAction)
    {
        if (setMoney == null)
        {
            setMoney = setMoneyAction;
        }
        else if (!setMoney.GetInvocationList().Contains(setMoneyAction))
        {
            setMoney += setMoneyAction;
        }
    }

    public void UnsubscribeOnSetMoney(Action<int> setMoneyAction)
    {
        setMoney -= setMoneyAction;
    }

    public void OnSetMoney(int money)
    {
        setMoney?.Invoke(money);
    }
    #endregion

    #region StopGame

    public void SubscribeOnStopGame(Action stopGameAction)
    {
        if (stopGame == null)
        {
            stopGame = stopGameAction;
        }
        else if (!stopGame.GetInvocationList().Contains(stopGameAction))
        {
            stopGame += stopGameAction;
        }
    }

    public void UnsubscribeOnStopGame(Action stopGameAction)
    {
        stopGame -= stopGameAction;
    }

    public void OnStopGame()
    {
        stopGame?.Invoke();
    }

    #endregion

    #region LoseGame
    public void SubscribeOnLoseGame(Action loseGameAction)
    {
        if (loseGame == null)
        {
            loseGame = loseGameAction;
        }
        else if (!loseGame.GetInvocationList().Contains(loseGameAction))
        {
            loseGame += loseGameAction;
        }
    }

    public void UnsubscribeOnLoseGame(Action loseGameAction)
    {
        loseGame -= loseGameAction;
    }

    public void OnLoseGame() 
    { 
        loseGame?.Invoke();
    }

    #endregion

    #region NewWave
    public void SubscribeOnNewWave(Action newWaveAction)
    {
        if (newWave == null)
        {
            newWave = newWaveAction;
        }
        else if (!newWave.GetInvocationList().Contains(newWaveAction))
        {
            newWave += newWaveAction;
        }
    }

    public void UnsubscribeOnNewWave(Action newWaveAction)
    {
        newWave -= newWaveAction;
    }

    public void OnNewWave()
    {
        newWave?.Invoke();
    }
    #endregion

    #region EndWave
    public void SubscribeOnEndWave(Action<int> endWaveAction)
    {
        if (endWave == null)
        {
            endWave = endWaveAction;
        }
        else if (!endWave.GetInvocationList().Contains(endWaveAction))
        {
            endWave += endWaveAction;
        }
    }

    public void UnsubscribeOnEndWave(Action<int> endWaveAction)
    {
        endWave -= endWaveAction;
    }

    public void OnEndWave(int timeToNextWave)
    {
        endWave?.Invoke(timeToNextWave);
    }
    #endregion

    #region UseBlowUp
    public void SubscribeOnUseBlowUp(Action<int, int> useBlowUpAction)
    {
        if (useBlowUp == null)
        {
            useBlowUp = useBlowUpAction;
        }
        else if (!useBlowUp.GetInvocationList().Contains(useBlowUpAction))
        {
            useBlowUp += useBlowUpAction;
        }
    }

    public void UnsubscribeOnUseBlowUp(Action<int, int> useBlowUpAction)
    {
        useBlowUp -= useBlowUpAction;
    }

    public void OnUseBlowUp(int granadeCount, int mineCount)
    {
        useBlowUp?.Invoke(granadeCount, mineCount);
    }
    #endregion

    #region ShowADV
    public void SubscribeOnShowADV(Action showADVAction)
    {
        if (showADV == null)
        {
            showADV = showADVAction;
        }
        else if (!showADV.GetInvocationList().Contains(showADVAction))
        {
            showADV += showADVAction;
        }
    }

    public void UnsubscribeShowADV(Action showADVAction)
    {
        showADV -= showADVAction;
    }

    public void OnShowADV()
    {
        showADV?.Invoke();
    }
    #endregion

    public override void Destroy()
    {
        Instance = null;
    }
}

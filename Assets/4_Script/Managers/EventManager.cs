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

    private Action<int, int,int> Shoot;
    private Action<Interactable> PlayerInteraction;
    private Action<HealthObject> Death;
    private Action<int> SetMoney;
    private Action StopGame;
    private Action LoseGame;
    private Action NewWave;
    private Action<int> EndWave;
    private Action<int> UseBlowUp;
    #region Shoot
    public void SubscribeOnShoot(Action<int,int,int> sender)
    {
        if(Shoot == null)
        {
            Shoot = sender;
        }else if (!Shoot.GetInvocationList().Contains(sender))
        {
            Shoot += sender;
        }
    }

    public void UnsubscribeOnShoot(Action<int,int,int> sender)
    {
        Shoot -= sender;
    }

    public void OnShoot(int currentPatronsInMagazine, int allPatrons, int maxPatronsInMagazine)
    {
        Shoot?.Invoke(currentPatronsInMagazine, allPatrons, maxPatronsInMagazine);
    }

    #endregion

    #region PlayerInteraction
    public void SubscribePlayerInteraction(Action<Interactable> sender)
    {
        if (PlayerInteraction == null)
        {
            PlayerInteraction = sender;
        }
        else if (!PlayerInteraction.GetInvocationList().Contains(sender))
        {
            PlayerInteraction += sender;
        }
    }

    public void UnsubscribePlayerInteraction(Action<Interactable> sender)
    {
        PlayerInteraction -= sender;
    }

    public void OnPlayerInteraction(Interactable interactable)
    {
        PlayerInteraction?.Invoke(interactable);
    }

    #endregion

    #region Death
    public void SubscribeOnDeath(Action<HealthObject> sender)
    {
        if (Death == null)
        {
            Death = sender;
        }
        else if (!Death.GetInvocationList().Contains(sender))
        {
            Death += sender;
        }
    }

    public void UnsubscribeOnDeath(Action<HealthObject> sender)
    {
        Death -= sender;
    }

    public void OnDeath(HealthObject obj)
    {
        Death?.Invoke(obj);
    }
    #endregion

    #region PauseGame

    public void SubscribeOnStopGame(Action sender)
    {
        if (StopGame == null)
        {
            StopGame = sender;
        }
        else if (!StopGame.GetInvocationList().Contains(sender))
        {
            StopGame += sender;
        }
    }

    public void UnsubscribeOnStopGame(Action sender)
    {
        StopGame -= sender;
    }

    public void OnStopGame()
    {
        StopGame?.Invoke();
    }

    #endregion

    #region LoseGame
    public void SubscribeOnLoseGame(Action sender)
    {
        if (LoseGame == null)
        {
            LoseGame = sender;
        }
        else if (!LoseGame.GetInvocationList().Contains(sender))
        {
            LoseGame += sender;
        }
    }

    public void UnsubscribeOnLoseGame(Action sender)
    {
        LoseGame -= sender;
    }

    public void OnLoseGame() 
    { 
        LoseGame?.Invoke();
    }

    #endregion

    #region SetMoney
    public void SubscribeOnSetMoney(Action<int> sender)
    {
        if (SetMoney == null)
        {
            SetMoney = sender;
        }
        else if (!SetMoney.GetInvocationList().Contains(sender))
        {
            SetMoney += sender;
        }
    }

    public void UnsubscribeOnSetMoney(Action<int> sender)
    {
        SetMoney -= sender;
    }

    public void OnSetMoney(int money)
    {
        SetMoney?.Invoke(money);
    }
    #endregion

    #region NewWave
    public void SubscribeOnNewWave(Action sender)
    {
        if (NewWave == null)
        {
            NewWave = sender;
        }
        else if (!NewWave.GetInvocationList().Contains(sender))
        {
            NewWave += sender;
        }
    }

    public void UnsubscribeOnNewWave(Action sender)
    {
        NewWave -= sender;
    }

    public void OnNewWave()
    {
        NewWave?.Invoke();
    }
    #endregion

    #region EndWave
    public void SubscribeOnEndWave(Action<int> sender)
    {
        if (EndWave == null)
        {
            EndWave = sender;
        }
        else if (!EndWave.GetInvocationList().Contains(sender))
        {
            EndWave += sender;
        }
    }

    public void UnsubscribeOnEndWave(Action<int> sender)
    {
        EndWave -= sender;
    }

    public void OnEndWave(int timeToNextWave)
    {
        EndWave?.Invoke(timeToNextWave);
    }
    #endregion

    #region UseBlowUp
    public void SubscribeOnUseBlowUp(Action<int> sender)
    {
        if (UseBlowUp == null)
        {
            UseBlowUp = sender;
        }
        else if (!UseBlowUp.GetInvocationList().Contains(sender))
        {
            UseBlowUp += sender;
        }
    }

    public void UnsubscribeOnUseBlowUp(Action<int> sender)
    {
        UseBlowUp -= sender;
    }

    public void OnUseBlowUp(int granadeCount)
    {
        UseBlowUp?.Invoke(granadeCount);
    }
    #endregion
    public override void Destroy()
    {
        Instance = null;
    }
}

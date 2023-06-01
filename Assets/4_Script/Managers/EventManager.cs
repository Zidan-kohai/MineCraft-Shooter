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
    private Action<bool> PlayerInteraction;

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
    public void SubscribePlayerInteraction(Action<bool> sender)
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

    public void UnsubscribePlayerInteraction(Action<bool> sender)
    {
        PlayerInteraction -= sender;
    }

    public void OnPlayerInteraction(bool isInteraction)
    {
        PlayerInteraction?.Invoke(isInteraction);
    }

    #endregion
}

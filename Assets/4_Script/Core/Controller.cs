using UnityEngine;
public class Controller : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private Manager[] managers;
    private void Awake()
    {
        foreach(var manager in managers)
        {
            manager.Init();
        }
    }
}

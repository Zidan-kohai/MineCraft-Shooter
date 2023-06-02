using System.Collections.Generic;
using UnityEngine;
public class Controller : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private List<Manager> managers;
    private void Awake()
    {
        foreach(var manager in managers)
        {
            manager.Init();
        }
    }

    private void OnDestroy()
    {
        for(int i = managers.Count - 1; i >= 0; i--)
        {
            managers[i].Destroy();
        }
    }
}

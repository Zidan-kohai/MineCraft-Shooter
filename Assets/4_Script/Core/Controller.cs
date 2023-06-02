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
}

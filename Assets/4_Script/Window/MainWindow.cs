using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : Window
{
    [Header("Bullets")]
    [SerializeField] private GameObject countPatronShowPanel;
    [SerializeField] private TMP_Text allPatrons;
    [SerializeField] private TMP_Text currentPatronsInMagazine;
    [SerializeField] private TMP_Text maxPatronsInMagazine;

    [Header("Interaction")]
    [SerializeField] private GameObject tapEToUse;

    private void Start()
    {
        EventManager.Instance.SubscribeOnShoot(OnShoot);
        EventManager.Instance.SubscribePlayerInteraction(TapEToUse);
    }
    private void OnShoot(int currentPatronsInMagazine, int allPatrons, int maxPatronsInMagazine)
    {
        this.allPatrons.text =  allPatrons.ToString();
        this.currentPatronsInMagazine.text = currentPatronsInMagazine.ToString();
        this.maxPatronsInMagazine.text = maxPatronsInMagazine.ToString();
    }

    private void TapEToUse(bool isInteraction)
    {
        tapEToUse.SetActive(isInteraction);
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnsubscribeOnShoot(OnShoot);
        EventManager.Instance.UnsubscribePlayerInteraction(TapEToUse);
    }
}

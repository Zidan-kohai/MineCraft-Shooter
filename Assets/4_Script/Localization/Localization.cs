using TMPro;
using UnityEngine;

public class Localization : MonoBehaviour
{
    [SerializeField] private string rus;
    [SerializeField] private string eng;
    [SerializeField] private TMP_Text TMP;

    public void ChangeLanguage(Langauge langauge)
    {
        switch (langauge)
        {
            case Langauge.Russian:
                TMP.text = rus;
                break;
            case Langauge.English:
                TMP.text = eng;
                break;
        }
    }
}

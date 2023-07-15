using TMPro;
using UnityEngine;

public class Localization : MonoBehaviour
{
    [SerializeField] private string rus;
    [SerializeField] private string eng;
    [SerializeField] private TMP_Text TMP;

    public void ChangeLanguage(Langauge langauge)
    {
        TMP.text = "";
        switch (langauge)
        {
            case Langauge.Russian:
                foreach (var item in rus)
                {
                    if (item == '/')
                    {
                        TMP.text += "\n";
                    }
                    else
                    {
                        TMP.text += item;
                    }
                }
                break;
            case Langauge.English:
                foreach (var item in eng)
                {
                    if (item == '/')
                    {
                        TMP.text += "\n";
                    }
                    else
                    {
                        TMP.text += item;
                    }
                }
                break;
        }
    }
}

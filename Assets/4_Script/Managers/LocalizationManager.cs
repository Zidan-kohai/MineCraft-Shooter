using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class LocalizationManager : Manager
{
    [SerializeField] TMP_Dropdown LanguageDropDown;
    [SerializeField] List<Localization> _localization;

    public override void AfterInit()
    {
        int LanguageIndex = (int)DataManager.Instance.GetLangauge();
        ChangeLanguages(LanguageIndex);
        LanguageDropDown.value = LanguageIndex;
    }

    public void ChangeLanguages(int LanguageIndex)
    {
        Langauge langauge = (Langauge)LanguageIndex;

        foreach (var item in _localization)
        {
            item.ChangeLanguage(langauge);
        }

        DataManager.Instance.SetLanguage(langauge);
    }
}
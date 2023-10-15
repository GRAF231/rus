using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class I18NManager
{
    int currentLanguageIndex = 0;
    public enum Language
    {
        en,
        ru,
    }

    public int LanguageCount = Enum.GetNames(typeof(Language)).Length;

    public event Action<Language> OnChangeLanguage;

    public Language Lang
    {
        get;
        private set;
    }

    public void ChangeLang()
    {
        currentLanguageIndex++;
        if (currentLanguageIndex == LanguageCount)
            currentLanguageIndex = 0;

        Lang = (Language)currentLanguageIndex;
        OnChangeLanguage?.Invoke(Lang);
        Debug.Log($"Language: {Lang}");
    }
}

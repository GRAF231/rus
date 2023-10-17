using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct LocalizedText
{
    public I18NManager.Language Language;
    public string Text;
}

public class I18NText : I18NBase
{
    TextMeshProUGUI _text;
    [SerializeField] LocalizedText[] localizedTexts = new LocalizedText[Managers.I18n.LanguageCount];
    Dictionary<I18NManager.Language, string> _textDictionary;

    protected new void Start()
    {
        Managers.I18n.OnChangeLanguage += ChangeLanguage;
        _text = GetComponent<TextMeshProUGUI>();
        _textDictionary = new Dictionary<I18NManager.Language, string>(localizedTexts.Length);

        foreach (var text in localizedTexts)
        {
            _textDictionary[text.Language] = text.Text;
        }
        ChangeLanguage(Managers.I18n.Lang);
    }

    protected override void ChangeLanguage(I18NManager.Language lang)
    {
        _text.text = _textDictionary[lang];
    }
}

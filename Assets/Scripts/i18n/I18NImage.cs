using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct LocalizedImage
{
    public I18NManager.Language Language;
    public Sprite Image;
}

public class I18NImage : I18NBase
{
    Image _image;
    [SerializeField] LocalizedImage[] localizedImages = new LocalizedImage[Managers.I18n.LanguageCount];
    Dictionary<I18NManager.Language, Sprite> _imageDictionary;

    protected new void Start()
    {
        Managers.I18n.OnChangeLanguage += ChangeLanguage;
        _image = GetComponent<Image>();
        _imageDictionary = new Dictionary<I18NManager.Language, Sprite>(localizedImages.Length);

        foreach (var image in localizedImages)
        {
            _imageDictionary[image.Language] = image.Image;
        }
        ChangeLanguage(Managers.I18n.Lang);
    }

    protected override void ChangeLanguage(I18NManager.Language lang)
    {
        _image.sprite = _imageDictionary[lang];
    }
}

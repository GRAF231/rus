using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class I18NBase : MonoBehaviour
{
    protected abstract void ChangeLanguage(I18NManager.Language lang);

    protected void Start()
    {
        Managers.I18n.OnChangeLanguage += ChangeLanguage;
    }

    private void OnDestroy()
    {
        Managers.I18n.OnChangeLanguage -= ChangeLanguage;
    }
}

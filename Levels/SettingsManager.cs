using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public List<Locale> locales;
    private Locale currentLocal;
    private int localeID;
    // Start is called before the first frame update
    void Start()
    {
        currentLocal = LocalizationSettings.SelectedLocale;
        localeID = locales.IndexOf(currentLocal);
    }

    public void nextLocaleSet()
    {
        if (++localeID < locales.Count)
        {
            LocalizationSettings.SelectedLocale = locales[localeID];
        }
        else
        {
            localeID = 0;
            LocalizationSettings.SelectedLocale = locales[localeID];
        }
    }

    public void previousLocaleSet()
    {
        if (--localeID >= 0)
        {
            LocalizationSettings.SelectedLocale = locales[localeID];
        }
        else
        {
            localeID = locales.Count-1;
            LocalizationSettings.SelectedLocale = locales[localeID];
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", volume);
    }
}

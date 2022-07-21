using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject controlSettings;
    [SerializeField]
    private GameObject audioSettings;

    public void Back()
    {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        SceneManager.LoadScene("StartMenu");
    }

    public void Audio()
    {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        controlSettings.SetActive(false);
        audioSettings.SetActive(true);
    }

    public void Controls()
    {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        audioSettings.SetActive(false);
        controlSettings.SetActive(true);
    }
}

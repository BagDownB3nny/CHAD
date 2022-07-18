using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    [SerializeField]
    private Slider masterVolume;
    [SerializeField]
    private Slider musicVolume;
    [SerializeField]
    private Slider soundEffectsVolume;

    private void Awake()
    {
        
    }

    public void MasterVolumeSlide()
    {
        MusicManager.instance.SetVolume((masterVolume.value * musicVolume.value)
                / (masterVolume.maxValue * musicVolume.maxValue));
        SoundManager.instance.SetVolume((masterVolume.value * soundEffectsVolume.value)
                / (masterVolume.maxValue * soundEffectsVolume.maxValue));
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
    }

    public void MusicVolumeSlide()
    {
        MusicManager.instance.SetVolume((masterVolume.value * musicVolume.value)
                / (masterVolume.maxValue * musicVolume.maxValue));
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
    }

    public void SoundEffectsVolumeSlide()
    {
        SoundManager.instance.SetVolume((masterVolume.value * soundEffectsVolume.value)
                / (masterVolume.maxValue * soundEffectsVolume.maxValue));
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
    }
}

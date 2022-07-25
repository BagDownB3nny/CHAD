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

    private bool isAwake = false;

    private void Awake()
    {
        masterVolume.value = AudioManager.instance.masterVolume * masterVolume.maxValue;
        musicVolume.value = MusicManager.instance.musicVolume * musicVolume.maxValue;
        soundEffectsVolume.value = SoundManager.instance.soundEffectsVolume * soundEffectsVolume.maxValue;
        isAwake = true;
    }

    public void MasterVolumeSlide()
    {
        if (isAwake) {
            AudioManager.instance.SetVolume(masterVolume.value / masterVolume.maxValue);
            MusicManager.instance.SetVolume(musicVolume.value / musicVolume.maxValue);
            SoundManager.instance.SetVolume(soundEffectsVolume.value / soundEffectsVolume.maxValue);
            SoundManager.instance.PlaySound(Sounds.ButtonPress);
        }
    }

    public void MusicVolumeSlide()
    {
        if (isAwake)
        {
            MusicManager.instance.SetVolume(musicVolume.value / musicVolume.maxValue);
            SoundManager.instance.PlaySound(Sounds.ButtonPress);
        }
    }

    public void SoundEffectsVolumeSlide()
    {
        if (isAwake)
        {
            SoundManager.instance.SetVolume(soundEffectsVolume.value / soundEffectsVolume.maxValue);
            SoundManager.instance.PlaySound(Sounds.ButtonPress);
        }
    }
}

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
        masterVolume.value = AudioManager.instance.masterVolume * masterVolume.maxValue;
        musicVolume.value = MusicManager.instance.musicVolume * musicVolume.maxValue;
        soundEffectsVolume.value = SoundManager.instance.soundEffectsVolume * soundEffectsVolume.maxValue;
    }

    public void MasterVolumeSlide()
    {
        AudioManager.instance.SetVolume(masterVolume.value / masterVolume.maxValue);
        MusicManager.instance.SetVolume(musicVolume.value / musicVolume.maxValue);
        SoundManager.instance.SetVolume(soundEffectsVolume.value / soundEffectsVolume.maxValue);
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
    }

    public void MusicVolumeSlide()
    {
        MusicManager.instance.SetVolume(musicVolume.value / musicVolume.maxValue);
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
    }

    public void SoundEffectsVolumeSlide()
    {
        SoundManager.instance.SetVolume(soundEffectsVolume.value / soundEffectsVolume.maxValue);
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
    }
}

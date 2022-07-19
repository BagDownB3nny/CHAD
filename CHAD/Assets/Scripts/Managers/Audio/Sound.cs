using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOverlapSound(AudioClip _audioClip, float _volume)
    {
        audioSource.clip = _audioClip;
        audioSource.volume = _volume;
        audioSource.Play();

        // Setting an ending time for audio clip
        float clipLength = _audioClip.length;
        StartCoroutine(EndAudio(clipLength));
    }

    public void PlayContinuousSound(AudioClip _audioClip, float _volume)
    {
        if (audioSource.isPlaying)
        {
            return;
        }
        else
        {
            audioSource.clip = _audioClip;
            audioSource.volume = _volume;
            audioSource.Play();
        }
    }

    private IEnumerator EndAudio(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        SoundManager.instance.EndAudio(gameObject);
    }
}

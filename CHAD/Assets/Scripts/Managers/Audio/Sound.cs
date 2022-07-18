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

    public void PlayOverlapSound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();

        // Setting an ending time for audio clip
        float clipLength = audioClip.length;
        StartCoroutine(EndAudio(clipLength));
    }

    public void PlayContinuousSound(AudioClip audioClip)
    {
        if (audioSource.isPlaying)
        {
            return;
        }
        else
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    private IEnumerator EndAudio(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        SoundManager.instance.EndAudio(gameObject);
    }
}

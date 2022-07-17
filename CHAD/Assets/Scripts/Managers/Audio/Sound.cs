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

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();

        // Setting an ending time for audio clip
        float clipLength = audioClip.length;
        StartCoroutine(EndAudio(clipLength));
    }

    private IEnumerator EndAudio(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        SoundManager.instance.EndAudio(gameObject);
    }
}

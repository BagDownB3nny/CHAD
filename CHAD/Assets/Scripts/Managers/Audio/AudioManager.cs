using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public float masterVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            masterVolume = 1f;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float _volume)
    {
        masterVolume = _volume;
    }
}

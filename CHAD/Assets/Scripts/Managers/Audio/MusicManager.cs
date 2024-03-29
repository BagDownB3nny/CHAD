using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Music
{
    none = 0,
    menu = 1,
    lobby = 2,
    game = 3
}

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> musicPrefabs;
    public static MusicManager instance;
    public float musicVolume;

    private Music currentlyPlaying;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            PlayMusic(Music.menu);
            musicVolume = 1f;

        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(Music music)
    {
        if (music == Music.none)
        {
            GetComponent<AudioSource>().Stop();
        }
        else if (currentlyPlaying != music)
        {
            GetComponent<AudioSource>().clip = musicPrefabs[(int)music - 1];
            GetComponent<AudioSource>().Play();
            currentlyPlaying = music;
        }
    }

    public void SetVolume(float _volume)
    {
        musicVolume = _volume;
        GetComponent<AudioSource>().volume = AudioManager.instance.masterVolume * musicVolume;
    }
}

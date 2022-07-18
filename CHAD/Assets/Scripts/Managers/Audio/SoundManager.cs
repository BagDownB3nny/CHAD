using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds
{
    None = 0,
    ButtonPress = 1,
    EquipGun = 2,
    Interact = 3,
    Walk = 4,
    Sprint = 5,
    TestRifleShot = 6,
    CrossbowShot = 7,

}

public enum SoundTypes
{
    Overlap = 1,
    Continuous = 2
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject soundPrefab;

    public static SoundManager instance;
    private Stack<GameObject> inactiveSounds;

    [SerializeField]
    private List<AudioClip> audioClips;

    private Dictionary<Sounds, SoundTypes> soundType;
    private Dictionary<Sounds, GameObject> continousSoundPlayers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSounds();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }



    private void InitializeSounds()
    {
        int soundCount = 100;
        inactiveSounds = new Stack<GameObject>();
        continousSoundPlayers = new Dictionary<Sounds, GameObject>();
        for (int i = 0; i < soundCount; i++)
        {
            GameObject newSound = Instantiate(soundPrefab, new Vector3(0, 0, 0)
                    , Quaternion.identity, transform);
            newSound.SetActive(false);
            inactiveSounds.Push(newSound);
        }
        soundType = new Dictionary<Sounds, SoundTypes>()
        {
            {Sounds.None, SoundTypes.Overlap },
            {Sounds.ButtonPress, SoundTypes.Overlap},
            {Sounds.EquipGun, SoundTypes.Overlap},
            {Sounds.Interact, SoundTypes.Overlap},
            {Sounds.Walk, SoundTypes.Continuous},
            {Sounds.Sprint, SoundTypes.Continuous},
            {Sounds.TestRifleShot, SoundTypes.Overlap},
            {Sounds.CrossbowShot, SoundTypes.Overlap}
        };
        foreach (KeyValuePair<Sounds, SoundTypes> sound in soundType)
        {
            if (sound.Value == SoundTypes.Continuous)
            {
                GameObject continousSoundPlayer = inactiveSounds.Pop();
                continousSoundPlayer.SetActive(true);
                continousSoundPlayers.Add(sound.Key, continousSoundPlayer);
            }
        }
    }

    public void PlaySound(Sounds _sound)
    {
        if (soundType[_sound] == SoundTypes.Overlap)
        {
            PlayOverlapSound(_sound);
        } else if (soundType[_sound] == SoundTypes.Continuous)
        {
            PlayContinuousSound(_sound);
        }
    }

    private void PlayOverlapSound(Sounds _sound)
    {
        GameObject sound = inactiveSounds.Pop();
        sound.SetActive(true);
        AudioClip audioClip = audioClips[(int)_sound];
        sound.GetComponent<Sound>().PlayOverlapSound(audioClip);
    }

    private void PlayContinuousSound(Sounds _sound)
    {
        GameObject sound = continousSoundPlayers[_sound];
        AudioClip audioClip = audioClips[(int)_sound];
        sound.GetComponent<Sound>().PlayContinuousSound(audioClip);
    }

    public void EndAudio(GameObject _sound)
    {
        _sound.SetActive(false);
        inactiveSounds.Push(_sound);
    }

    private void EndWalkSound(GameObject _sound)
    {

    }
}

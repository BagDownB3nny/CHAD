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

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject soundPrefab;

    public static SoundManager instance;
    private Stack<GameObject> inactiveSounds;

    [SerializeField]
    private List<AudioClip> audioClips;

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
        for (int i = 0; i < soundCount; i++)
        {
            GameObject newSound = Instantiate(soundPrefab, new Vector3(0, 0, 0)
                    ,Quaternion.identity, transform);
            newSound.SetActive(false);
            inactiveSounds.Push(newSound);
        }
    }

    public void PlaySound(Sounds _sound)
    {
        Debug.Log("PLAYING " + _sound);
        GameObject sound = inactiveSounds.Pop();
        sound.SetActive(true);
        AudioClip audioClip = audioClips[(int)_sound];
        sound.GetComponent<Sound>().PlaySound(audioClip);
    }

    public void EndAudio(GameObject _sound)
    {
        _sound.SetActive(false);
        inactiveSounds.Push(_sound);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyInputs
{
    None = 0,
    MoveUp = 1,
    MoveDown = 2,
    MoveLeft = 3,
    MoveRight = 4,
    Interact = 5,
    Sprint = 6,
    ChangeWeapon = 7
}

public class InputChanger : MonoBehaviour
{
    [SerializeField]
    GameObject keyListener;
    [SerializeField]
    KeyInputs keyType;

    public void OnFANCYClick()
    {
        keyListener.SetActive(true);
        keyListener.GetComponent<KeyListener>().ListeningForKey(keyType);
        Debug.Log("CLICKING");
    }
}

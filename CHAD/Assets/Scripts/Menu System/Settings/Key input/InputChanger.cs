using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputChanger : MonoBehaviour
{
    [SerializeField]
    GameObject keyListener;
    [SerializeField]
    PlayerInputs playerInput;

    private void Awake()
    {
        GameObject text = transform.GetChild(0).gameObject;
        text.GetComponent<TMP_Text>().text = InputManager.instance.keybinds[playerInput].ToString();
    }

    public void OnClick()
    {
        keyListener.SetActive(true);
        keyListener.GetComponent<KeyListener>().ListeningForKey(gameObject, playerInput);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyListener : MonoBehaviour
{
    private bool isListening = false;
    private PlayerInputs listeningForKey;
    private GameObject currentButton;

    private void Update()
    {
        if (isListening)
        {
            if (Input.anyKeyDown)
            {
                KeyCode keyCode = GetKeyCode();
                if (keyCode != KeyCode.None)
                {
                    ChangeKeybind(keyCode);
                    End();
                }
            }
        }
    }

    private KeyCode GetKeyCode()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
            {
                return kcode;
            }
        }
        return KeyCode.None;
    }

    private void ChangeKeybind(KeyCode _keyCode)
    {
        InputManager.instance.ChangeKeybind(listeningForKey, _keyCode);
        GameObject text = currentButton.transform.GetChild(0).gameObject;
        text.GetComponent<TMP_Text>().text = _keyCode.ToString();
    }

    private void End()
    {
        isListening = false;
        listeningForKey = PlayerInputs.None;
        gameObject.SetActive(false);
    }

    public void ListeningForKey(GameObject _button, PlayerInputs _key)
    {
        currentButton = _button;
        isListening = true;
        listeningForKey = _key;
    }
}

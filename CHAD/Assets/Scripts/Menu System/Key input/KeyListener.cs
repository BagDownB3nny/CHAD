using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyListener : MonoBehaviour
{
    private bool isListening = false;
    private KeyInputs listeningForKey;
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
                Debug.Log("KeyCode down: " + kcode);
                return kcode;
            }
        }
        return KeyCode.None;
    }

    private void ChangeKeybind(KeyCode _keyCode)
    {
        //InputManager.instance.ChangeKeybind(listeningForKey, keybind);
        GameObject text = currentButton.transform.GetChild(0).gameObject;
        text.GetComponent<TMP_Text>().text = _keyCode.ToString();
    }

    private void End()
    {
        isListening = false;
        listeningForKey = KeyInputs.None;
        gameObject.SetActive(false);
    }

    public void ListeningForKey(GameObject _button, KeyInputs _key)
    {
        currentButton = _button;
        isListening = true;
        listeningForKey = _key;
    }
}

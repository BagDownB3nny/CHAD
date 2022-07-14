using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyListener : MonoBehaviour
{
    private bool isListening = false;
    private KeyInputs listeningForKey;

    private void Update()
    {
        if (isListening)
        {
            if (Input.anyKeyDown)
            {
                string keybind = Input.inputString;
                //InputManager.instance.ChangeKeybind(listeningForKey, keybind);
                Debug.Log(keybind);
                End();
            }
        }
    }

    private void End()
    {
        isListening = false;
        listeningForKey = KeyInputs.None;
        gameObject.SetActive(false);
    }

    public void ListeningForKey(KeyInputs _key)
    {
        isListening = true;
        listeningForKey = _key;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerInputs
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

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Dictionary<PlayerInputs, KeyCode> keybinds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetDefaultKeybinds();
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeKeybind(PlayerInputs _input, KeyCode _keycode)
    {
        keybinds[_input] = _keycode;
    }

    private void SetDefaultKeybinds()
    {
        keybinds = new Dictionary<PlayerInputs, KeyCode>()
        {
            { PlayerInputs.MoveUp, KeyCode.W },
            { PlayerInputs.MoveDown, KeyCode.S },
            { PlayerInputs.MoveLeft, KeyCode.A },
            { PlayerInputs.MoveRight, KeyCode.D },
            { PlayerInputs.Interact, KeyCode.E },
            { PlayerInputs.Sprint, KeyCode.LeftShift },
            { PlayerInputs.ChangeWeapon, KeyCode.Q },
        };
    }
}

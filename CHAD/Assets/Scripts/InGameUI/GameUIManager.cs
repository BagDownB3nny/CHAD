using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public GameObject healthBar;
    public GameObject weaponIcon;
    public GameObject pauseMenu;
    public GameObject weaponWheel;
    public GameObject crosshair;
    public GameObject interactText;
    public GameObject objectiveText;
    public GameObject holeUI;
    public GameObject settingsMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (!pauseMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
            }
        }
        if (Input.GetKey(InputManager.instance.keybinds[PlayerInputs.ChangeWeapon])) {
            weaponWheel.SetActive(true);
            crosshair.GetComponent<SpriteRenderer>().enabled = false;
        } else {
            if (weaponWheel.activeSelf) {
                weaponWheel.GetComponent<WeaponWheel>().currentButton.GetComponent<WeaponSelectButton>().EquipGun();
            }
            weaponWheel.SetActive(false);
            crosshair.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void SetWeaponIcon(Sprite _weapon) {
        weaponIcon.GetComponent<Image>().sprite = _weapon;
    }

    public void InstantiaitePlayerPointer(GameObject _player, GameObject _playerPointer, int _playerId) {
        GameObject newPlayerPointer = Instantiate(_playerPointer, _player.transform.position, Quaternion.identity, transform);
        newPlayerPointer.GetComponent<PlayerPointer>().target= _player;
        newPlayerPointer.GetComponent<TextMeshProUGUI>().text = "P" + _playerId.ToString();
    }

    public void SettingsBack()
    {
        settingsMenu.SetActive(false);
    }
}

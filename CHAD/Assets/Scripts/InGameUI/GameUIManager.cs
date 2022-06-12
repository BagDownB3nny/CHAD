using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject healthBar;
    public GameObject weaponIcon;
    public GameObject pauseMenu;
    public GameObject weaponWheel;
    public GameObject crosshair;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
        if (Input.GetKey(KeyCode.Q)) {
            weaponWheel.SetActive(true);
            crosshair.SetActive(false);
        } else {
            if (weaponWheel.activeSelf) {
                weaponWheel.GetComponent<WeaponWheel>().currentButton.GetComponent<WeaponSelectButton>().EquipGun();
            }
            weaponWheel.SetActive(false);
            crosshair.SetActive(true);
        }
    }

    public void SetWeaponIcon(Sprite _weapon) {
        weaponIcon.GetComponent<Image>().sprite = _weapon;
    }
}

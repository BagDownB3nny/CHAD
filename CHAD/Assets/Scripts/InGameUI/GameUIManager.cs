using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject interactText;
    public GameObject objectiveText;
    public GameObject holeUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
        if (Input.GetKey(KeyCode.Q)) {
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheel : MonoBehaviour
{
    private float startingAngle = -157.5f;
    public List<GameObject> weaponButtons;
    public Button currentButton;
    void Update()
    {
        foreach (GameObject button in weaponButtons) {
            button.GetComponent<Button>().image.color = new Color(255,255,255,152);
        }
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized;
        float rotation = Mathf.Atan2(cursorPosition.y, cursorPosition.x) * Mathf.Rad2Deg;
        float weaponButton = Mathf.Floor((rotation - startingAngle)/45) + 1;
        currentButton = weaponButtons[(int)weaponButton].GetComponent<Button>();
        currentButton.image.color = new Color(0,0,0,94);
    }

    public void UpdateWeaponButton(int weaponButton, GameObject gun) {
        weaponButton -= 2;
        if (weaponButton < 0) {
            weaponButton += 8;
        }
        weaponButtons[weaponButton].transform.GetChild(0).GetComponent<Image>().sprite 
                = gun.GetComponent<SpriteRenderer>().sprite;
        weaponButtons[weaponButton].transform.GetChild(0).GetComponent<Image>().enabled = true;
    }
}

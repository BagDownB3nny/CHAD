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
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 middleOfScreen = Camera.main.transform.position;
        Vector2 screenToCursorVector = cursorPosition - middleOfScreen;
        float rotation = Mathf.Atan2(screenToCursorVector.y, screenToCursorVector.x) * Mathf.Rad2Deg;
        float weaponButton = Mathf.Floor((rotation - startingAngle)/45) + 1;
        currentButton = weaponButtons[(int)weaponButton].GetComponent<Button>();
        currentButton.image.color = new Color(0,0,0,94);
    }

    public void UpdateWeaponButton(int weaponButton, GameObject gun) {
        weaponButton -= 3;
        if (weaponButton < 0) {
            weaponButton += 8;
        }
        weaponButtons[weaponButton].transform.GetChild(0).GetComponent<Image>().sprite 
                = gun.GetComponent<SpriteRenderer>().sprite;
        weaponButtons[weaponButton].transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void ResetWheel()
    {
        for (int i = 0; i < weaponButtons.Count; i++)
        {
            weaponButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            weaponButtons[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }
}

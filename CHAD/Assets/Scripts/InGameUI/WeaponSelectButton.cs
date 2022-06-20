using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectButton : MonoBehaviour
{
    public int gunIndex;
    public Button button;
    public Time colorChange;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(EquipGun);
    }

    public void EquipGun() {
        if (GameManager.instance.players.ContainsKey(PlayerClient.instance.myId.ToString())) {
            GameManager.instance.players[PlayerClient.instance.myId.ToString()]
                    .GetComponent<PlayerWeaponsManager>().EquipGun(gunIndex);
        }
    }

}

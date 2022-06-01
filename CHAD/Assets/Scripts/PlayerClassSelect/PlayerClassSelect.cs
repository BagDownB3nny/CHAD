using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassSelect : MonoBehaviour
{
    public PlayerClasses playerClass;

    private bool colliding;
 
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            colliding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            colliding = false;
        }
    }
    
    private void Update() {
        if(colliding && Input.GetKeyDown(KeyCode.Space) && NetworkManager.gameType == GameType.Client) {
            GameManager.instance.SendChangeClass(playerClass);
        }
    }
}

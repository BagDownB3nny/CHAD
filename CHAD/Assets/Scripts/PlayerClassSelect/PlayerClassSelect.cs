using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassSelect : MonoBehaviour
{
    public PlayerClasses playerClass;

    private bool colliding;
 
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log(playerClass + " colliding");
            colliding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log(playerClass + " no longer colliding");
            colliding = false;
        }
    }
    
    private void Update() {
        if(colliding && Input.GetKeyDown(KeyCode.E) && NetworkManager.gameType == GameType.Client) {
            Debug.Log("Sending class change to " + playerClass);
            GameManager.instance.SendChangeClass(playerClass);
        }
    }
}

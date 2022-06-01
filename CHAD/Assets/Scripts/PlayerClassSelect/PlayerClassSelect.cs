using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassSelect : MonoBehaviour
{
    public PlayerClasses playerClass;

    private bool colliding;
 
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && NetworkManager.IsMine(other.gameObject.GetComponent<CharacterStatsManager>().characterRefId)) {
            colliding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && NetworkManager.IsMine(other.gameObject.GetComponent<CharacterStatsManager>().characterRefId)) {
            colliding = false;
        }
    }
    
    private void Update() {
        if(colliding && Input.GetKeyDown(KeyCode.Space)) {
            GameManager.instance.SendChangeClass(playerClass);
        }
    }
}

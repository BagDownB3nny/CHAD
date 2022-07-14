using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public abstract class Interactable : MonoBehaviour
{
    GameObject interactText;

    private void Awake() {
        interactText = GameUIManager.instance.interactText;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerItemsManager>().interact += OnInteract;

            if (NetworkManager.IsMine(collision.gameObject.GetComponent<PlayerStatsManager>().characterRefId)) {
                interactText.GetComponent<TextMeshProUGUI>().text = GetText();
                interactText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerItemsManager>().interact -= OnInteract;

            if (NetworkManager.IsMine(collision.gameObject.GetComponent<PlayerStatsManager>().characterRefId)) {
                interactText.SetActive(false);
            }
        }
    }

    public abstract void OnInteract(GameObject player);

    public abstract string GetText();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Drops : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerItemsManager>().interact += PickUp;
            Debug.Log("Weapon can be picked up");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerItemsManager>().interact -= PickUp;
            Debug.Log("Weapon cannot be picked up");
        }
    }

    public abstract void PickUp(GameObject player);
}

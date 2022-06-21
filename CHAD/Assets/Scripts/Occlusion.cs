using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occlusion : MonoBehaviour
{   
    public float alpha;

    SpriteRenderer sprite;
    private void Start() {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
 
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy") {
            if (other.gameObject.GetComponent<Renderer>().sortingOrder < gameObject.GetComponent<Renderer>().sortingOrder) {
                sprite.color = new Color(1,1,1,alpha);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy") {
            sprite.color = new Color(1,1,1,1);
        }
    }
}

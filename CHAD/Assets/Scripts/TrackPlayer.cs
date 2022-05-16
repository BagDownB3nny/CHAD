using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    //TODO: implement randomised tracking if there are multiple players in the future
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 1f;
    private Rigidbody2D enemy;

    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    void Update(){
        Vector3 direction = player.position - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        direction.Normalize();
        face(angle);
        track((Vector2) direction);
    }
   
    void face(float angle) {
        if (angle >= -90 && angle < 89) {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        } else {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void track(Vector2 direction){
        enemy.MovePosition((Vector2) transform.position + (direction * speed * Time.deltaTime));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    //TODO: implement randomised tracking if there are multiple players in the future
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 1f;
    private Rigidbody2D enemy;
    private Vector2 direction;
    private float heading;

    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    void Update(){
        direction = player.position - this.transform.position;
        direction.Normalize();
        heading = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    private void FixedUpdate() {
        face(heading);
        track(direction);
    }
   
    void face(float heading) {
        if (heading >= -90 && heading < 89) {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        } else {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void track(Vector2 direction){
        enemy.MovePosition((Vector2) this.transform.position + (direction * speed * Time.deltaTime));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //scripts needed
    PlayerStatsManager statsManagerScript;
    PlayerWeaponsManager weaponManagerScript;

    public Vector2 movement;
    public float speed;
    Rigidbody2D playerRb;
    public Vector2 movementSum;

     private void Awake() {
        //get the statsmanager and ask for the movement stats
        statsManagerScript = gameObject.GetComponent<PlayerStatsManager>();
        statsManagerScript.UpdateMovementStats();
        Debug.Log("PLAYER: transferred movement stats from stats manager to movement");

        weaponManagerScript = gameObject.GetComponent<PlayerWeaponsManager>();
        Debug.Log("PLAYER: set reference to weapon manager script in movement");
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        //movement.Normalize();
    }

    private void FixedUpdate()
    {
        Vector2 _position = (Vector2)this.transform.position + movement * speed * Time.deltaTime;
        Debug.Log(movement);
        playerRb.MovePosition(_position);
        ServerSend.PlayerPosition(1, _position);
    }
    public void SetMovementStats(float _speed) {
        speed = _speed;
    }
}

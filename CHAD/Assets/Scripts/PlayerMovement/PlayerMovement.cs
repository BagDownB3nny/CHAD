using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //scripts needed
    PlayerStatsManager statsManagerScript;
    PlayerWeaponsManager weaponManagerScript;

    public float speed;
    Rigidbody2D playerRb;

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
        Vector2 movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        bool[] _input = new bool[4];
        if (movement.y > 0) {_input[0] = true;}
        if (movement.x < 0) {_input[1] = true;}
        if (movement.y < 0) {_input[2] = true;}
        if (movement.x > 0) {_input[3] = true;}

        ClientSend.MovePlayer(_input);
    }

    public Vector2 MovePlayer(Vector2 _movement) {
        playerRb.MovePosition((Vector2) transform.position + _movement * speed * Time.deltaTime);
        Debug.Log("moved server player");
        return (Vector2) transform.position;
    }

    public void SetMovementStats(float _speed) {
        speed = _speed;
    }
}

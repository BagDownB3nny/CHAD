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
        SendMovement();
    }

    //client sends the input to server
    public void SendMovement() {
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

    //client receives processed game state from server and sets it
    public void ReceiveMovement(Vector2 _position) {
        transform.position = _position;
    }

    //server decodes the input received from client, changes its own game state and sends the game state back to client
    public Vector2 MovePlayer(bool[] _input) {
        Vector2 _movement = new Vector3(0, 0, 0);
        if (_input[0]) { _movement.y += 1; }
        if (_input[1]) { _movement.x -= 1; }
        if (_input[2]) { _movement.y -= 1; }
        if (_input[3]) { _movement.x += 1; }
        _movement.Normalize();

        playerRb.MovePosition((Vector2) transform.position + _movement * speed * Time.deltaTime);
        return (Vector2) transform.position;
    }

    public void SetMovementStats(float _speed) {
        speed = _speed;
    }
}

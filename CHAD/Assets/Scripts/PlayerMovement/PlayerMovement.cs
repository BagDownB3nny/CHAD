using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //scripts needed
    PlayerStatsManager playerStatsManager;
    Rigidbody2D playerRb;

    //OnPlayerMove delegate
    public delegate void OnPlayerMove(GameObject player);
    public OnPlayerMove onPlayerMove;

     private void Awake() {
         playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (NetworkManager.IsMine(playerStatsManager.characterRefId))
        {
            SendMovement();
        }
    }
#region MovePlayer
    //client sends the input to server
    private void SendMovement() {
        Vector2 movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool[] _input = new bool[5];
        if (movement.y > 0) {_input[0] = true;}
        if (movement.x < 0) {_input[1] = true;}
        if (movement.y < 0) {_input[2] = true;}
        if (movement.x > 0) {_input[3] = true;}
        if (isSprinting) { _input[4] = true;}
        ClientSend.MovePlayer(_input);
    }

    //server decodes the input received from client,
    //changes its own game state and sends the game state back to client
    public Vector2 MovePlayer(bool[] _input) {
        Vector2 _movement = new Vector3(0, 0, 0);
        if (_input[0]) { _movement.y += 1; }
        if (_input[1]) { _movement.x -= 1; }
        if (_input[2]) { _movement.y -= 1; }
        if (_input[3]) { _movement.x += 1; }
        _movement.Normalize();

        if (_input[4]) { _movement *= 2; ServerSend.Broadcast("YOU ARE SPRINTING!"); }

        playerRb.MovePosition((Vector2) transform.position + _movement * playerStatsManager.speed * Time.deltaTime);
        return (Vector2) transform.position;
    }

    //client receives processed game state from server and sets it
    public void ReceiveMovement(Vector2 _position) {
        transform.position = _position;
        if (NetworkManager.IsMine(GetComponent<PlayerStatsManager>().characterRefId) && onPlayerMove != null)
        {
            onPlayerMove(gameObject);
        }
    }
    #endregion
}

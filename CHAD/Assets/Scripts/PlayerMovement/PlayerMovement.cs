using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //scripts needed
    PlayerStatsManager statsManagerScript;
    PlayerWeaponsManager weaponManagerScript;

    Vector2 movement;
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
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //movement.Normalize();
    }

    private void FixedUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        bool[] _input = new bool[4];
        if (movement.y > 0)
        {
            _input[0] = true;
        }
        if (movement.x < 0)
        {
            _input[1] = true;
        }
        if (movement.y < 0)
        {
            _input[2] = true;
        }
        if (movement.x > 0)
        {
            _input[3] = true;
        }
        ClientSend.SPAM();
        ClientSend.PlayerMovement(_input);
        Debug.Log("TRYING TO FKING SPAM");
        //playerRb.MovePosition((Vector2) this.transform.position + movement * speed * Time.deltaTime);
    }

    public void SetMovementStats(float _speed) {
        speed = _speed;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    GameObject player;

    float boundX = 5.0f;
    float boundY = 5.0f;
    float speed = player.speed;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x + boundX < player.transform.position.x)
        {
            
        }
    }
}

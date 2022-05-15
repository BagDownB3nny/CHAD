using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public GameObject player;

    float boundX = 3.0f;
    float boundY = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
        transform.Translate(Vector3.back);
    }

    void LateUpdate()
    {
        if (transform.position.x + boundX < player.transform.position.x)
        {
            transform.position = new Vector3(player.transform.position.x - boundX, transform.position.y, 0);
            transform.Translate(Vector3.back);
        }
        if (transform.position.x - boundX > player.transform.position.x)
        {
            transform.position = new Vector3(player.transform.position.x + boundX, transform.position.y, 0);
            transform.Translate(Vector3.back);
        }
        if (transform.position.y + boundY < player.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y - boundY, 0);
            transform.Translate(Vector3.back);
        }
        if (transform.position.y - boundY > player.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y + boundY, 0);
            transform.Translate(Vector3.back);
        }
    }
}

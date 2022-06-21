using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMapCam : MonoBehaviour
{
    public float speed;
    public float zoomSpeed;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.up * speed);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.down * speed);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * speed);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * speed);
        }
        if (Input.GetKey(KeyCode.RightArrow) && Camera.main.orthographicSize >= 1) {
            Camera.main.orthographicSize -= zoomSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            Camera.main.orthographicSize += zoomSpeed;
        }

    }
}

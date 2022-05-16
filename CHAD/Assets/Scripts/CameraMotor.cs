using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public GameObject player;

    //float boundX = 3.0f;
    //float boundY = 1.5f;

    [SerializeField] private float radialBound;
    [SerializeField] private float cameraSpeedMultiplier = 0.9f;
    [SerializeField] private float mouseBiasAmount = 0.5f;
    [SerializeField] private float maxMouseBiasDist = 3f;
    private Player playerScript;
    private float cameraSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //get access to player script
        GameObject player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();

        //center camera to player
        transform.position = player.transform.position;
        transform.Translate(Vector3.back);

        
    }

    void LateUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //update camera speed based on player speed
        cameraSpeed = playerScript.speed * cameraSpeedMultiplier;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        //move cam to player if out of bounds
        if (distance > radialBound) {
            Vector3 desiredCamPos = 
                new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, desiredCamPos, cameraSpeed * Time.deltaTime);
        }

        float camToPlayerDist = Vector3.Distance(player.transform.position, transform.position);

        //bias cam to mouse position
        if (!(camToPlayerDist > maxMouseBiasDist)) {
            Vector3 biasedCamPos = 
                new Vector3(mousePos.x, mousePos.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, biasedCamPos, mouseBiasAmount * Time.deltaTime);
        }
    }
}

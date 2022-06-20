using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public GameObject player;
    private bool playerDead = false;

    //float boundX = 3.0f;
    //float boundY = 1.5f;

    [SerializeField] private float radialBound;
    [SerializeField] private float cameraSpeedMultiplier = 0.9f;
    [SerializeField] private float mouseBiasAmount = 0.5f;
    [SerializeField] private float maxMouseBiasDist = 3f;
    private PlayerStatsManager playerScript;
    private float cameraSpeed;

    public static CameraMotor instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (NetworkManager.gameType == GameType.Client)
            {
                PlayerSpawner.onPlayerSpawn += OnPlayerSpawn;
            }
        }
    }
   

    public void OnPlayerSpawn(int _playerId)
    {
        if (NetworkManager.IsMine(_playerId.ToString()))
        {
            //assigns player
            player = GameManager.instance.players[PlayerClient.instance.myId.ToString()];
            playerScript = player.GetComponent<PlayerStatsManager>();

            //center camera to player
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

            //observe player movement
            player.GetComponent<PlayerMovement>().onPlayerMove += MoveCamera;
        }
    }

    public void MoveCamera(GameObject _player)
    {
        if (!playerDead && NetworkManager.IsMine(_player.GetComponent<PlayerStatsManager>().characterRefId) && player != null) {
            transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -10);
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //player = _player;
            //playerScript = player.GetComponent<PlayerStatsManager>();

            ////update camera speed based on player speed
            //cameraSpeed = playerScript.speed * cameraSpeedMultiplier;

            //float distance = Vector3.Distance(player.transform.position, transform.position);

            ////move cam to player if out of bounds
            //if (distance > radialBound) {
            //    Vector3 desiredCamPos = 
            //        new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            //    transform.position = Vector3.MoveTowards(transform.position, desiredCamPos, cameraSpeed * Time.deltaTime);
            //}

            //float camToPlayerDist = Vector3.Distance(player.transform.position, transform.position);

            ////bias cam to mouse position
            //if (!(camToPlayerDist > maxMouseBiasDist)) {
            //    Vector3 biasedCamPos = 
            //        new Vector3(mousePos.x, mousePos.y, transform.position.z);
            //    transform.position = Vector3.Lerp(transform.position, biasedCamPos, mouseBiasAmount * Time.deltaTime);
            //}
        }
    }

    public void DeclarePlayerDead() {
        playerDead = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public GameObject player;
    public bool playerDead = false;
    public float speed = 0.1f;
    public float zoomSpeed = 0.1f;

    //float boundX = 3.0f;
    //float boundY = 1.5f;

    [SerializeField] private float radialBound;
    [SerializeField] private float cameraSpeedMultiplier = 0.9f;
    [SerializeField] private float mouseBiasAmount = 0.5f;
    [SerializeField] private float maxMouseBiasDist = 3f;
    private PlayerStatsManager playerScript;
    private float cameraSpeed;

    public static CameraMotor instance;
    bool isServerCam = false;

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

        if (NetworkManager.gameType == GameType.Server) {
            isServerCam = true;
        }
    }

    private void Start() {
        if (isServerCam) {
            Camera.main.orthographicSize = 40;
        }
    }
    
    void Update()
    {
        if (playerDead) {
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

    public void SetPlayerDeath(bool deathStatus) {
        if (!deathStatus) {
            Camera.main.orthographicSize = 9;
        }
        playerDead = deathStatus;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;
    public GameObject playerPrefab;
    public Dictionary<int, GameObject> players;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            players = new Dictionary<int, GameObject>();
            Server.Start(4, 26950);      
        } else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
        
    }

    public GameObject InitializePlayer(int _id)
    {
        Debug.Log("Spawning player!");
        return Instantiate(playerPrefab);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

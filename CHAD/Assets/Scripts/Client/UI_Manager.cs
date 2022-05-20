using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    public GameObject startMenu;
    //public InputField usernameField;

    public void Awake()
    {
        if (instance == null )
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        Debug.Log("Attempting to connect");
        startMenu.SetActive(false);
        //usernameField.interactable = false;
        Client.instance.ConnectToServer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour
{
    public PlayerItems playerItem;

    public abstract void OnPickUp(string _playerRefId);
}

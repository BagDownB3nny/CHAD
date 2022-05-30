using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //these will be manually set by the weapons manager when it instantiates this weapon
    [Header("Holder Parameters")]
    public GameObject holder;

    public void Discard() {
        Destroy(gameObject);
    }
}

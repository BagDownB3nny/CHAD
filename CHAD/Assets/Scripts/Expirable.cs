using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expirable : MonoBehaviour
{
    public float shelfLife = 5;
    private float timeToExpiry;

    private void Start() {
        timeToExpiry = shelfLife;
    }

    void Update()
    {
        if (timeToExpiry <= 0) {
            Destroy(gameObject);
        } else {
            timeToExpiry -= Time.deltaTime;
        }
    }
}

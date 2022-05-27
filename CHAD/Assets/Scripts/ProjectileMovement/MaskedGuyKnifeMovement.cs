using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedGuyKnifeMovement : MonoBehaviour, ProjectileMovement
{
    ProjectileStatsManager statsManagerScript;
    
    private void Awake() {
        statsManagerScript = gameObject.GetComponent<ProjectileStatsManager>();
    }
    void Start()
    {
        if (NetworkManager.gameType == GameType.Server) {
            gameObject.GetComponent<Rigidbody2D>().velocity = 
                (Vector2) statsManagerScript.projectileDirectionVector * statsManagerScript.speed;
        }
    }

    //move to targetLocation and destroy if reached
    void FixedUpdate()
    {
        if (NetworkManager.gameType == GameType.Server) {
            float distanceTravelled = (transform.position - statsManagerScript.originLocationVector).magnitude;
            if (distanceTravelled > statsManagerScript.range) {
                DestroyProjectile();
            } else {
                SendMove();
            }
        }
    }

    public void SendMove() {
        ServerSend.MoveProjectile(statsManagerScript.projectileRefId, transform.position);
    }

    public void ReceiveMovement(Vector2 _position) {
        transform.position = _position;
    }

    public void DestroyProjectile() {
        ServerSend.DestroyProjectile(statsManagerScript.projectileRefId);
        Destroy(gameObject);
    }

    public void ReceiveDestroyProjectile() {
        Destroy(gameObject);
    }
}

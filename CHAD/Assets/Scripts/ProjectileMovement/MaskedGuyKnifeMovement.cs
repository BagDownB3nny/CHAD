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
        Face();
        if (NetworkManager.gameType == GameType.Server) {
            gameObject.GetComponent<Rigidbody2D>().velocity = 
                (Vector2) statsManagerScript.directionVector * statsManagerScript.speed;
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
        ServerSend.MoveProjectile(statsManagerScript.projectileId, transform.position);
    }

    public void ReceiveMovement(Vector2 _position) {
        transform.position = _position;
    }

    //point projectile towards target
    public void Face() {
        float directionRotation = Mathf.Atan2(statsManagerScript.directionVector.y, 
            statsManagerScript.directionVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, directionRotation + statsManagerScript.rotationOffset);
    }

    public void DestroyProjectile() {
        //ServerSend.DestroyProjectile(statsManagerScript.id);
        Destroy(gameObject);
    }

    public void ReceiveDestroyProjectile() {
        Destroy(gameObject);
    }
}

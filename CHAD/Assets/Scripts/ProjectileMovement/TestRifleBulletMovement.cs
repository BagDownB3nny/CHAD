using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRifleBulletMovement : MonoBehaviour, ProjectileMovement
{
    //scripts needed
    ProjectileStatsManager projectileStatsManager;

    [Header("Projectile Parameters")]
    public float rotationOffset = -90;
    
    private void Awake() {
        projectileStatsManager = gameObject.GetComponent<ProjectileStatsManager>();
    }

    void Start()
    {
        Face();
        if (NetworkManager.gameType == GameType.Server) {
            gameObject.GetComponent<Rigidbody2D>().velocity = 
                ((Vector2) projectileStatsManager.directionVector).normalized * projectileStatsManager.speed;
        }
    }

    //move to targetLocation and destroy if reached
    void FixedUpdate()
    {
        if (NetworkManager.gameType == GameType.Server) {
            float distanceTravelled = (transform.position - projectileStatsManager.originLocationVector).magnitude;
            if (distanceTravelled > projectileStatsManager.range) {
                DestroyProjectile();
            } else {
                SendMove();
            }
        }
    }

    public void SendMove() {
        ServerSend.MoveProjectile(projectileStatsManager.projectileRefId, transform.position);
    }

    public void ReceiveMovement(Vector2 _position) {
        transform.position = _position;
    }

    //point projectile towards target
    public void Face() {
        Vector2 directionVector = projectileStatsManager.directionVector;
        float directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, directionRotation + rotationOffset);
    }

    public void DestroyProjectile() {
        ServerSend.DestroyProjectile(projectileStatsManager.projectileRefId);
        Destroy(gameObject);
    }

    public void ReceiveDestroyProjectile() {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileMovement : MonoBehaviour
{
    //scripts needed
    public ProjectileStatsManager projectileStatsManager;

    [Header("Projectile Parameters")]
    public float rotationOffset = -90;
    
    private void Awake() {
        projectileStatsManager = gameObject.GetComponent<ProjectileStatsManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (NetworkManager.gameType == GameType.Server && other.tag.Equals("Wall")) {
            DestroyProjectile();
        }
    }

    public void Move() {
        ServerSend.MoveProjectile(projectileStatsManager.projectileRefId, transform.position);
    }

    public void ReceiveMovement(Vector2 _position) {
        transform.position = _position;
    }

    public void DestroyProjectile() {
        ServerSend.DestroyProjectile(projectileStatsManager.projectileRefId);
        Destroy(gameObject);
        GameManager.instance.projectiles.Remove(projectileStatsManager.projectileRefId);
    }

    public void ReceiveDestroyProjectile() {
        Destroy(gameObject);
        GameManager.instance.projectiles.Remove(projectileStatsManager.projectileRefId);
    }
}

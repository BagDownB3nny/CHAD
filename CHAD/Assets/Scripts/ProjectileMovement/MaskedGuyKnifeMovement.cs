using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedGuyKnifeMovement : MonoBehaviour, ProjectileMovement
{
    ProjectileStatsManager statsManagerScript;

    [Header("Projectile Parameters")]
    public float speed;
    public float range;
    public GameObject origin;
    public Vector3 originLocationVector;
    public Vector3 directionVector;
    public float rotationOffset;
    
    private void Awake() {
        statsManagerScript = gameObject.GetComponent<ProjectileStatsManager>();
    }
    void Start()
    {
        Face();
        if (NetworkManager.gameType == GameType.Server) {
            gameObject.GetComponent<Rigidbody2D>().velocity = (Vector2) directionVector * speed;
        }
    }

    //move to targetLocation and destroy if reached
    void FixedUpdate()
    {
        if (NetworkManager.gameType == GameType.Server) {
            float distanceTravelled = (transform.position - originLocationVector).magnitude;
            if (distanceTravelled > range) {
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
        float directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, directionRotation + rotationOffset);
    }

    public void DestroyProjectile() {
        //ServerSend.DestroyProjectile(statsManagerScript.id);
        Destroy(gameObject);
    }

    public void ReceiveDestroyProjectile() {
        Destroy(gameObject);
    }

    public void SetStats(float _speed, float _range, GameObject _origin, 
            Vector3 _originLocationVector, Vector3 _directionVector, float _rotationOffset) {
        speed = _speed;
        range = _range;
        origin = _origin;
        originLocationVector = _originLocationVector;
        directionVector = _directionVector;
        rotationOffset = _rotationOffset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 movement;
    public float speed = 5f;
    BoxCollider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 toMove = movement.normalized * speed * Time.fixedDeltaTime;
        float distance = toMove.magnitude;
        RaycastHit2D hitX = Physics2D.BoxCast(
            transform.position, coll.size, 0, new Vector3(toMove.x, 0, 0), distance, LayerMask.GetMask("Collidable"));
        if (hitX.collider == null)
        {
            transform.Translate(new Vector3(toMove.x, 0, 0));
        }
        RaycastHit2D hitY = Physics2D.BoxCast(
            transform.position, coll.size, 0, new Vector3(0, toMove.y, 0), distance, LayerMask.GetMask("Collidable"));
        if (hitY.collider == null)
        {
            transform.Translate(new Vector3(0, toMove.y, 0));
        }
    }
}

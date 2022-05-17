using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 movement;
    public float speed = 5.0f;
    Rigidbody2D rb;
    public List<GameObject> guns = new List<GameObject>(8);
    public GameObject currentGun;
    public GameObject defaultGun;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //AddGun(defaultGun);
        guns[0] = defaultGun;
        EquipGun(0);
        Debug.Log("equipping gun");
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
    }

    private void FixedUpdate()
    {
        rb.MovePosition((Vector2) this.transform.position + movement * speed * Time.deltaTime);
    }

    //instantiate a selected gun
    public void EquipGun(int gunIndex) {
        if (currentGun != null) {
            currentGun.GetComponent<Gun>().Discard();
        }      
        currentGun = Instantiate(guns[gunIndex], transform.position, Quaternion.identity, transform);
        Debug.Log("equipped" + gunIndex);
    }

    //adds gun to empty slot
    public bool AddGun(GameObject gun) {
        if (guns.Count < 8) {
            guns.Add(gun);
            return true;
        }
        return false;
    }

    //discards a gun
    public void DiscardGun(int gunIndex) {
        guns[gunIndex] = null;
    }
}

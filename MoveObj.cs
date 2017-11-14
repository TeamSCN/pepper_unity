using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Rigidbody playerRididbody;
    int floorMask;

    //起動後の処理
    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        playerRididbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
    }

    //移動
    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;

        playerRididbody.MovePosition(transform.position + movement);
    }

}


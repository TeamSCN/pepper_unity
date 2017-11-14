using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

	public float speed = 3.0f;
    float moveX = 0f;
    float moveZ = 0f;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal") * speed;
        moveZ = Input.GetAxis("Vertical") * speed;
        Vector3 direction = new Vector3(moveX, 0, moveZ);

        controller.SimpleMove(direction);
    }
}

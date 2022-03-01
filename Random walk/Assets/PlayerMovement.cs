using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 100f;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0f;
        if (Input.GetKey("q"))
        {
            y += 50f * Time.deltaTime;
        }
        if (Input.GetKey("z"))
        {
            y -= 50f * Time.deltaTime;
        }

        Vector3 move = transform.right * x + transform.forward * z + transform.up*y;
        controller.Move(move * speed * Time.deltaTime );
    }
}

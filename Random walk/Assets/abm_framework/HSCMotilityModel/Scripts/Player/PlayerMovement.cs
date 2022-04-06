using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 100f;
    public float mouseSensitivity = 500f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = new Vector3(0f, 0f, -679f);
    }

    // Update is called once per frame
    void Update()
    {
        // 'WASD' to move in xy plane + 'QZ' to move along y axis
        Vector3 move = Vector3.zero;
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

        move = transform.right * x + transform.forward * z + transform.up*y;


        // Rotation - Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Attractor vector towards origin of cell cluster - press SPACE to move towards origin
        Vector3 attractor = (this.transform.position * -1.0f).normalized * 1f;

        if (Input.GetKey("space"))
        {//reorient towards center
            transform.forward = attractor.normalized;
            controller.Move((attractor) * speed * Time.deltaTime);
        }
        else if (move != Vector3.zero)
        {//planar input motion (WASD-QZ)
            controller.Move((move) * speed * Time.deltaTime );
        }
        else
        {//drifting motion
            controller.Move(this.transform.forward * speed/5f * Time.deltaTime);
        }
        
    }
}

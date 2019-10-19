using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public LineRenderer line;
    public GameObject anchor;

    [Header("Look")]
    public Camera mainCamera;
    public float mouseSensitivity = 2;
    public float upDownRange;
    public float snappiness = 1;
    public bool lockMouseMovement = false;
    [Header("Movement")]
    public float speed = 5;

    Rigidbody rb;
    float rotX;
    float rotY;
    float xVelocity;
    float yVelocity;

    Vector3 ninty = new Vector3(90, 0, 0);
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        DrawLine();
        line.SetPosition(line.positionCount-1, transform.position);

        Look();
        Move();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    void Look()
    {
        rotX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        xVelocity = Mathf.Lerp(xVelocity, rotX, snappiness * Time.deltaTime);
        yVelocity = Mathf.Lerp(yVelocity, rotY, snappiness * Time.deltaTime);

        if (!lockMouseMovement)
        {
            //RotY
            rotY = Mathf.Clamp(rotY, -upDownRange, upDownRange);
            mainCamera.transform.localRotation = Quaternion.Euler(yVelocity, 0, 0);

            //RotX
            transform.Rotate(0, xVelocity, 0);
        }
    }

    void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * speed) + (transform.right * Input.GetAxis("Horizontal") * speed));
    }

    void DrawLine()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 1))
            {
                Instantiate(anchor, hit.point, Quaternion.FromToRotation(anchor.transform.up, hit.normal) * anchor.transform.rotation);
                line.SetPosition(line.positionCount - 1, hit.point);
                line.positionCount++;
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            if (line.positionCount > 1)
            {
                line.positionCount--;
            }
        }
    }
}

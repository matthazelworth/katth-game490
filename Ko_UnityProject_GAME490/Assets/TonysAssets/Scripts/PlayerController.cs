using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 5;
    public float turnSpeed = 10;
    public float hieght = 0.5f;
    public float heightPadding = 0.05f;
    public LayerMask ground;
    public float maxGroundAngle = 120;
    public bool debug;

    Vector2 input;
    float angle;
    float groundAngle;

    Quaternion targetRotation;
    Transform cam;

    Vector3 forward;
    RaycastHit hitInfo;
    bool grounded;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        GetInput();
        CalculateDirection();
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();
        DrawDebugLines();

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1)
        {
            return;
        }

        Rotate();
        Move();

    }

    void GetInput() //Input is based on Horizontal (a,d,<,>) and Vertcial (w,s,^,v) keys
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void CalculateDirection() //Direction relative to the camera's rotation
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    void Rotate() //Rotate twoards the calcualated angle
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void Move() //Player only moves along it own foward axis
    {
        if(groundAngle >= maxGroundAngle)
        {
            return;
        }
        transform.position += forward * velocity * Time.deltaTime;
    }

    void CalculateForward() //If the player is not grounded, forward will be equal to transfrom forward
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(transform.right, hitInfo.normal);
    }

    void CalculateGroundAngle()
    {
        if(!grounded)
        {
            groundAngle = 90;
            return;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    void CheckGround()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, hieght + heightPadding, ground))
        {
            if (Vector3.Distance(transform.position, hitInfo.point) < hieght)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * hieght, 5 * Time.deltaTime);
            }
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void ApplyGravity()
    {
        if(!grounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }

    void DrawDebugLines()
    {
        if(!debug)
        {
            return;
        }

        Debug.DrawLine(transform.position, transform.position + forward * hieght * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * hieght, Color.green);
    }

}

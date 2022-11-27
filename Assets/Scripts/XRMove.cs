using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRMove : MonoBehaviour
{
    public float moveSpeed = 5;
    public float rotateAngle = 15;

    Rigidbody body;
    float rotateTime = 0;
    Transform xrCamera;
    CapsuleCollider bodyCollider;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        xrCamera = Camera.main.transform;
        bodyCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {

        if (rotateTime > 0)
        {
            rotateTime -= Time.deltaTime;
            return;
        }
        Vector2 input = XRControllers.RightJoystick;
        if (Mathf.Abs(input.x) < 0.2f)
            return;

        float angle = input.x * rotateAngle;
        rotateTime = 0.15f;

        transform.Rotate(0, angle, 0);
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(XRControllers.LeftJoystick.x, 0, XRControllers.LeftJoystick.y);

        input = Quaternion.AngleAxis(xrCamera.eulerAngles.y, Vector3.up) * input;
        input.y = 0;

        body.velocity = input * moveSpeed;
    }

    private void LateUpdate()
    {
        bodyCollider.center = new Vector3(xrCamera.localPosition.x, bodyCollider.center.y, xrCamera.localPosition.z);
    }
}

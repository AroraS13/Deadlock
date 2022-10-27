using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public float moveSpeed = 5;
    public float turnSpeed = 10;
    public float smoothSpeed = 0.5f;

    Quaternion targetRotation;
    Vector3 targetPos;
    bool smoothRotation = false;

    Vector2 rotation = Vector2.zero;
    public float speed = 3;

    private void Update()
    {
        MoveWithTarget();
        LookAtTarget();

        /*rotation.y += Input.GetAxis(KeyCode.Joystick);
        rotation.x += Input.GetAxis("Mouse Y");
        transform.eulerAngles = (Vector2)rotation * speed;*/


        if (Input.GetKeyDown(KeyCode.G) && !smoothRotation)
        {
            StartCoroutine("RotateAroundTarget", 45);
        }

        if (Input.GetKeyDown(KeyCode.H) && !smoothRotation)
        {
            StartCoroutine("RotateAroundTarget", -45);
        }
    }
    void MoveWithTarget()
    {
        targetPos = target.position + offsetPos;
        targetPos.y = targetPos.y + 15;
        //Debug.Log(targetPos.y);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void LookAtTarget()
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        targetRotation.x -= (float)0.2;
       // Debug.Log(targetRotation.y);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

    }
    IEnumerator RotateAroundTarget(float angle)
    {
        Debug.Log(angle);
        Vector3 vel = Vector3.zero;
        Vector3 targetOffsetPos = Quaternion.Euler(0, angle, 0) * offsetPos;
        float dist = Vector3.Distance(offsetPos, targetOffsetPos);
        smoothRotation = true;

        while (dist > 0.02f)
        {
            offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, smoothSpeed);
            dist = Vector3.Distance(offsetPos, targetOffsetPos);
            yield return null;
        }
        smoothRotation = false;
        offsetPos = targetOffsetPos;
    }

}
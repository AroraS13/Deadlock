using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullPlayerMovementTest : MonoBehaviour
{

    public float speed;
    public float rotationSpeed;

    Animator anim;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputx = Input.GetAxis("Horizontal");
        float inputy = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(inputx, 0, inputy);
        movementDirection.Normalize();

        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            anim.SetBool("isMoving", true);

        }
        else{
            anim.SetBool("isMoving", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public float velocity = 7;
    public float turnSpeed = 10;

    Vector2 input;
    float angle;

    private Quaternion targetRotation;
    private Transform cam;

    Animator anim;

    bool velocityChanged = false;

    public Vector3 jump;
    public float jumpForce = 2.0f;
    public bool isGrounded;
    Rigidbody rb;

    private void Start()
    {
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.5f, 0.0f);
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {

        GetInput();

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1) return;

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        //CalculateDirection();
        //Rotate();
        Move();
    }
    void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        anim.SetFloat("BlendX", input.x);
        anim.SetFloat("BlendY", input.y);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetTrigger("TriggerRun");
            anim.ResetTrigger("TriggerWalk");
            StartCoroutine(changeSprint((float)0.5));
            


        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            if (velocityChanged)
            {
                velocity = (float)5.25;
            }
            else
            {
                velocity = 7;
            }
            
            anim.ResetTrigger("TriggerRun");
            anim.SetTrigger("TriggerWalk");            
            
        }
    }

    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        Debug.Log(angle);
        if (angle >= -45 && angle <= 45)
        {
            angle += cam.eulerAngles.y;
        Rotate();
        }
        
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

    }

    void Move()
    {
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!velocityChanged)
            {
                velocity = velocity * (float)0.75;
                velocityChanged = true;
            }
            transform.position += -transform.forward * velocity * Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (velocityChanged)
            {
                velocity = (float)1.333333333334 * velocity;
                velocityChanged = false;
            }
            transform.position += transform.forward * velocity * Time.deltaTime;


        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.Rotate(0, input.x, 0);

            transform.Translate(input.x * velocity * Time.deltaTime,0, 0);
            //transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * velocity, Space.World);
        }
        /*        if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * velocity, Space.World);
                }*/

    }

    IEnumerator changeSprint(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        velocity = 20;
    }
}

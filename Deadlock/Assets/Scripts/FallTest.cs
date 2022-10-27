using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FallTest : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    Vector2 input;

    private Transform cam;
    Animator anim;

    Rigidbody rb;

    bool crouching;
    bool sprinting; 
    bool velocityChanged = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        crouching = false;
        sprinting = false;
        anim.SetBool("FirstFall", true);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("IDLENEW"));
        Movement();
        GetInput();
        if (transform.position.y < 18)
        {
            anim.SetTrigger("OnGround");
            StartCoroutine(changeFall((float)2));
            
        }


    }
    void Movement()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
        {
            speed = 0;
            rotationSpeed = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
        {
            speed = 0;
            rotationSpeed = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Lookaround"))
        {
            speed = 0;
            rotationSpeed = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("IDLENEW"))
        {
            speed = 0;
            rotationSpeed = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchIdle"))
        {
            speed = 0;
            rotationSpeed = 900;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("WalkNew"))
        {
            speed = 6;
            rotationSpeed = 900;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("RunNew"))
        {
            speed = 10;
            rotationSpeed = 900;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slide 0"))
        {
            speed = 15;
            rotationSpeed = 900;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("CrouchNew"))
        {
            speed = 4;
            rotationSpeed = 900;

        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("RollNew"))
        {
            speed = 10;
            rotationSpeed = 900;
        }
    }

    void GetInput()
    {

        //Get Input from Unity
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(input.x, 0, input.y);
        movementDirection.Normalize();

        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        //Check for input, if so, toggle isMoving

        if (movementDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        
            
        }
        else
        {
            anim.SetBool("isMoving", false);
        
        }

        //Configure animator with Input
        anim.SetFloat("BlendX", input.x);
        anim.SetFloat("BlendY", input.y);

        //Check if sprint button is pressed and sprint
        if  (Input.GetButtonDown("Sprint"))
        {
            anim.SetBool("isSprinting", true);
            sprinting = true;
            StartCoroutine(changeCrouch((float)0.5));

            
        }
        if (Input.GetButtonUp("Sprint"))
        {
            anim.SetBool("isSprinting", false);
            sprinting = false;
        }
        
        //Check if crouch button is pressed and crouch
        if (Input.GetButtonDown("Crouch") && !crouching)
        {
            anim.SetBool("isCrouching", true);
            crouching = true;
            Debug.Log(crouching);
            if (sprinting){
                crouching = false;
                StartCoroutine(changeCrouch((float)0.5));
            }
        }
        else if (Input.GetButtonDown("Crouch") && crouching)
        {
            anim.SetBool("isCrouching", false);
            crouching = false;
            Debug.Log(crouching);
        }

        //Check if roll button is pressed and roll
        if (Input.GetButtonDown("Roll"))
        {
            anim.SetBool("isRolling", true);
        }
        if (Input.GetButtonUp("Roll"))
        {
            anim.SetBool("isRolling", false);
        }
        
        //Check if jump button is pressed and roll
        if (Input.GetButtonDown("Jump") )
        {
            anim.SetBool("isJumping", true);
            
        }
        if (Input.GetButtonUp("Jump") )
        {
            anim.SetBool("isJumping", false);
            
        }

        // if (Input.GetButtonDown("Crouch"))
        // {
        //     Crouch();
        //     //Debug.Log("Crouching??");
        // }
        // if (Input.getButtonUp("Crouch"))
        // {
        //     anim.SetBool("isCrouching", false);
        // }
        
    }

    void Sprint()
    {
        
    }

    // void Crouch()
    // {
    //     anim.setBool("isCrouching", true);
    // }

    IEnumerator changeCrouch(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        anim.SetBool("isCrouching", false);
    }
    IEnumerator changeFall(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        anim.SetBool("FirstFall", false);
    }

}

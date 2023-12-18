using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using System;
using Mono.Cecil;

public class PlayerMovement : MonoBehaviour
{
    public Transform mainCamera;
    public Rigidbody rb;
    public float moveSpeed, speedMultiplier;
    public bool isPC, isPhone;
    public static bool isGrounded, isUnderwater;
    public static event Action movePlayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movePlayer = PCControlInputs;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (Input.GetMouseButton(0)) isPC = true;
        if(isPC)
        { movePlayer?.Invoke(); }       */

        PCControlInputs();
        PhoneControlInputs();
        CheckGravityMultiplier();
        
    }

    void PCControlInputs()
    {
        Debug.Log("Player can now move");
        

        float zMove = Input.GetAxis("Vertical");
        float xMove = Input.GetAxis("Horizontal");

        Vector3 playerMoveDir = mainCamera.forward * zMove + mainCamera.right * xMove;

        Vector3 upwardForce = 5 * moveSpeed * Vector3.up;
        
        if (xMove != 0 || zMove != 0)//direction of view
        {
            rb.transform.forward = playerMoveDir;
            upwardForce = Vector3.zero;

            if (isGrounded)//register movement input when not airborne
            {
                upwardForce = 5 * moveSpeed * Vector3.up;
            }

            rb.AddForce(speedMultiplier * moveSpeed * playerMoveDir.normalized + upwardForce);
        }
        else return;

    }

    void CheckGravityMultiplier()
    {
        if (isGrounded)
        {
            rb.drag = 2.5f;
        }
        else
        {
            if (rb.drag > 0)
            {
                rb.drag -= 0.05f;
            }
            else rb.drag = 0;

        }
    }

    void PhoneControlInputs()
    {
        float screenWidth = Screen.width;

        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < screenWidth / 2)
            {
                Debug.Log("right screen touched");
            }
            else if (touch.position.x > screenWidth / 2)
            {
                Debug.Log("left screen touched");
            }
            else return;
        }

        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}

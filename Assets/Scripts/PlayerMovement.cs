using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public Transform mainCamera;
    public Rigidbody rb;
    public float moveSpeed, speedMultiplier, airTime;
    public bool isPC;
    public static bool isGrounded, isUnderwater;
    public static event Action movePlayer;
    public Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movePlayer = PCControlInputs;
        if (Application.isMobilePlatform)
        {
            isPC = false;
        }
        else isPC= true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (Input.GetMouseButton(0)) isPC = true;
        if(isPC)
        { movePlayer?.Invoke(); }       */

        if(isPC)
        {
            PCControlInputs();
        }
        else PhoneControlInputs();

        CheckGravityMultiplier();
        
    }

    void PCControlInputs()
    {
            float zMove = Input.GetAxis("Vertical");
            float xMove = Input.GetAxis("Horizontal");

            Vector3 playerMoveDir = mainCamera.forward * zMove + mainCamera.right * xMove;

            if (xMove != 0 || zMove != 0)//direction of view
            {
                rb.transform.forward = playerMoveDir;
                Vector3 upwardForce = Vector3.zero;

                if (isGrounded)//register movement input when not airborne
                {
                    upwardForce = 5 * moveSpeed * Vector3.up;
                }

                rb.AddForce(speedMultiplier * moveSpeed * playerMoveDir.normalized + upwardForce);
            }
            else return;

    }

    IEnumerator SwitchToUnderwater()
    {
        yield return new WaitForSeconds(1);
        rb.useGravity = false;
    }
    void CheckGravityMultiplier()
    {
        if (isGrounded||isUnderwater)
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

        if(!isGrounded&&!isUnderwater)
        {
            airTime += Time.deltaTime;

            if(airTime>3)
            {
                airTime = 0;
                rb.useGravity=true;
                rb.AddForce(0, 0.5f*-speedMultiplier*moveSpeed, 0.5f*speedMultiplier*moveSpeed);
            }
        }
    }

    void PhoneControlInputs()
    {
            joystick.gameObject.SetActive(true);

            float zMove = joystick.Vertical;
            float xMove = joystick.Horizontal;

            Vector3 playerMoveDir = mainCamera.forward * zMove + mainCamera.right * xMove;

            if (xMove != 0 || zMove != 0)//direction of view
            {
                rb.transform.forward = playerMoveDir;
                Vector3 upwardForce = Vector3.zero;

                if (isGrounded)//register movement input when not airborne
                {
                    upwardForce = 5 * moveSpeed * Vector3.up;
                }

                rb.AddForce(speedMultiplier * moveSpeed * playerMoveDir.normalized + upwardForce);
            }
            else return;
        
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Lake"))
        {
           isUnderwater= true;
            StartCoroutine(SwitchToUnderwater());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lake"))
        {
            isUnderwater = false;
            rb.useGravity = true;
            
        }
    }

}

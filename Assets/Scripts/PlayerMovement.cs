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
    public float movSpeed, startJump;
    public bool isPC, isPhone;
    public static event Action movePlayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startJump = 0.75f;
        movePlayer = PCControlInputs;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) isPC = true;
        if(isPC)
        { movePlayer?.Invoke(); }       
    }


    void PCControlInputs()
    {
        Debug.Log("Player can now move");
        if (Input.GetKey("w"))
        {
            rb.transform.forward = mainCamera.forward;
            startJump += Time.deltaTime;
            if (startJump >= 0.75f)
            {
                startJump = 0;
                rb.AddForce((mainCamera.forward * 100 + new Vector3(0, 100, 0)) * movSpeed);
            }
        }
        if (Input.GetKeyUp("w")) startJump = 0.75f;

        if (Input.GetKey("s"))
        {
            rb.transform.forward = -mainCamera.forward;
            startJump += Time.deltaTime;
            if (startJump >= 0.75f)
            {
                startJump = 0;
                rb.AddForce((-mainCamera.forward * 100 + new Vector3(0, 100, 0)) * movSpeed);
            }
        }
        if (Input.GetKeyUp("s")) startJump = 0.75f;

        if (Input.GetKey("a"))
        {
            rb.transform.forward = -mainCamera.right;
            startJump += Time.deltaTime;
            if (startJump >= 0.75f)
            {
                startJump = 0;
                rb.AddForce((-mainCamera.right * 100 + new Vector3(0, 100, 0)) * movSpeed);
            }
        }
        if (Input.GetKeyUp("a")) startJump = 0.75f;
        if (Input.GetKey("d"))
        {
            rb.transform.forward = mainCamera.right;
            startJump += Time.deltaTime;
            if (startJump >= 0.75f)
            {
                startJump = 0;
                rb.AddForce((mainCamera.right * 100 + new Vector3(0, 100, 0)) * movSpeed);
            }
        }
        if (Input.GetKeyUp("d")) startJump = 0.75f;

    }

    void PhoneControlInputs()
    {

    }

   
}

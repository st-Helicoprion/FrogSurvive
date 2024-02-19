using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform mainCamera;
    public Rigidbody rb;
    public float moveSpeed, speedMultiplier, airTime;
    public bool isPC;
    public static bool isGrounded, isUnderwater;
    public Joystick joystick;

    [Header("Audio")]
    public AudioSource playerAudioSource;
    public AudioSource waterAudioSource;
    public AudioClip footstepAudio, enterWaterAudio,
                     exitWaterAudio, waterMoveAudio;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
      
        if (Application.isMobilePlatform)
        {
            isPC = false;
        }
        else isPC= true;

        isUnderwater= false;
        isGrounded= false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(isPC)
        {
            PCControlInputs();
        }
        else PhoneControlInputs();

        CheckGravityMultiplier();
        UnderwaterMovementAudio();
    }

    void PCControlInputs()
    {
            joystick.gameObject.SetActive(false);

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

            if(airTime>2)
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

    void UnderwaterMovementAudio()
    {
        if(isUnderwater)
        {
            if(Mathf.Abs(rb.velocity.z) >2)
            {
                if (!playerAudioSource.isPlaying)
                {
                    playerAudioSource.pitch = Random.Range(1, 1.2f);
                    playerAudioSource.PlayOneShot(waterMoveAudio);

                }
            }
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
            if (!playerAudioSource.isPlaying)
            {
                playerAudioSource.pitch = Random.Range(1, 1.2f);
                playerAudioSource.volume = 0.5f;
                playerAudioSource.PlayOneShot(footstepAudio);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Lake"))
        {
           isUnderwater= true;
            if (!waterAudioSource.isPlaying)
            {   
                waterAudioSource.pitch = Random.Range(1, 1.2f);
                waterAudioSource.volume = 0.5f;
                waterAudioSource.PlayOneShot(enterWaterAudio);

            }
            StartCoroutine(SwitchToUnderwater());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lake"))
        {
            isUnderwater = false;
            rb.useGravity = true;
            if (!waterAudioSource.isPlaying)
            {
                waterAudioSource.pitch = Random.Range(1, 1.2f);
                waterAudioSource.volume = 0.2f;
                waterAudioSource.PlayOneShot(exitWaterAudio);

            }

        }
    }

}

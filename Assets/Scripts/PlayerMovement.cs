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
    public static bool isGrounded, isUnderwater, facePlant, hop;
    public Joystick joystick;

    [Header("Audio")]
    public AudioSource playerAudioSource;
    public AudioSource waterEnterAudioSource, waterExitAudioSource;
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
        facePlant = false;
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
                Vector3 downwardForce = Vector3.zero;

            if(isGrounded)
            {
                upwardForce = 5 * moveSpeed * Vector3.up;
            }
              
            if (airTime > 0&&!hop)
            {
                downwardForce = 0.5f*airTime*moveSpeed * Vector3.down;
            }
            else if (airTime > 0 && hop)
            {
                downwardForce = 0.2f * airTime * moveSpeed * Vector3.down;
            }

            if (!hop||PlayerSonarManager.strike)
            {
                if (facePlant)
                {
                    upwardForce = 5 * moveSpeed * transform.up;
                    rb.AddForce(-50 * playerMoveDir.normalized + upwardForce);
                }

                else

                    rb.AddForce(speedMultiplier * moveSpeed * playerMoveDir.normalized + upwardForce + downwardForce);

            }
            else return;

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
        }
    }

    void PhoneControlInputs()
    {
            if(AppUtilsManager.startUp)
            {
                joystick.gameObject.SetActive(false);
            }
            else
                joystick.gameObject.SetActive(true);

            float zMove = joystick.Vertical;
            float xMove = joystick.Horizontal;

        Vector3 playerMoveDir = mainCamera.forward * zMove + mainCamera.right * xMove;

        if (xMove != 0 || zMove != 0)//direction of view
        {
            rb.transform.forward = playerMoveDir;
            Vector3 upwardForce = Vector3.zero;
            Vector3 downwardForce = Vector3.zero;

            

            if (isGrounded)
            {
                upwardForce = 5 * moveSpeed * Vector3.up;
            }

            if (airTime > 0 && !hop)
            {
                downwardForce = 0.5f * airTime * moveSpeed * Vector3.down;
            }
            else if (airTime > 0 && hop)
            {
                downwardForce = 0.2f * airTime * moveSpeed * Vector3.down;
            }

            if (!hop||PlayerSonarManager.strike)
            {
                if (facePlant)
                {
                    upwardForce = 5 * moveSpeed * transform.up;
                    rb.AddForce(-50 * playerMoveDir.normalized + upwardForce);
                }

                else


                    rb.AddForce(speedMultiplier * moveSpeed * playerMoveDir.normalized + upwardForce + downwardForce);

            }
            else return;


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
                    playerAudioSource.volume = 0.2f;
                    playerAudioSource.PlayOneShot(waterMoveAudio);

                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
            airTime = 0;
            hop = false;
            PlayerSonarManager.strike= false;
            
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
            airTime = 0;
            hop = false;  
            if(!waterEnterAudioSource.isPlaying)
            {
                waterEnterAudioSource.pitch = Random.Range(1, 1.2f);
                waterEnterAudioSource.volume = 0.5f;
                waterEnterAudioSource.PlayOneShot(enterWaterAudio);
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
            if(!waterExitAudioSource.isPlaying)
            {
                waterExitAudioSource.pitch = Random.Range(1, 1.2f);
                waterExitAudioSource.volume = 0.2f;
                waterExitAudioSource.PlayOneShot(exitWaterAudio);
            }

        }
    }

}

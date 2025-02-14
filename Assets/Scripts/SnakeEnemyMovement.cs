using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemyMovement : MonoBehaviour
{
    private Transform playerPos;
    public Rigidbody[] rbArray;
    public float huntRadius, moveSpeed, slitherOffset, airTime,
                 attackInterval, attackCountdown,
                 slitherInterval, slitherCountdown,
                 locateInterval, locateCountdown,
                 idleAudioInterval, idleAudioCountdown;
    public static bool recoverAfterAttack, alert, isGrounded, isUnderwater;
    public int orientation;
    public Vector3 newRotation;
    public Vector2 huntRadiusRange;

    [Header("Audio")]
    public AudioSource snakeAudioSource;
    public AudioClip attackAudioClip;
    public AudioClip[] idleAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
       if(!AppUtilsManager.isPaused)
        {
            rbArray[12].AddForce(2 * moveSpeed * Vector3.down);
            rbArray[16].AddForce(2 * moveSpeed * Vector3.down);

            CheckBody();

            if (recoverAfterAttack)
            {
                recoverAfterAttack = false;
                StartCoroutine(RecoverAfterAttack());
            }
            else
            {
                slitherCountdown -= Time.deltaTime;

                if (slitherCountdown < 0)
                {
                    slitherCountdown = slitherInterval;
                    StartCoroutine(SlitherAnimation());
                }
            }

            if (Vector3.Distance(transform.position, playerPos.position) < huntRadius && !alert)
            {
                alert = true;
                snakeAudioSource.spatialBlend = 0.2f;
            }
            else snakeAudioSource.spatialBlend = 0.5f;

            if (alert)
            {
                SwitchToAlert();
                
                attackCountdown -= Time.deltaTime;

                if (attackCountdown < 0)
                {
                    AttackPlayer();
                }
            }
            else
            {
                CheckGravityMultiplier();
                GroundSticking();

                rbArray[0].AddForce(6 * moveSpeed * transform.forward);

                locateCountdown -= Time.deltaTime;
                idleAudioCountdown -=Time.deltaTime;

                if (locateCountdown < 0)
                {
                    MoveToPlayer();
                }

                if(idleAudioCountdown<0&&!snakeAudioSource.isPlaying)
                {
                    PlayIdleAudio();
                }
            }

            if (Vector3.Distance(transform.position, playerPos.position) > 150)
            {
                huntRadius = huntRadiusRange.y;
                snakeAudioSource.spatialBlend = 1;
            }
        }
       
    }

    void GroundSticking()
    {
        if (!isGrounded&&!alert)
        {
            rbArray[0].AddForce(4 * moveSpeed * Vector3.down);
           
        }
        else return;
        
    }

    void MoveToPlayer()
    {
        locateCountdown = locateInterval;
        transform.LookAt(playerPos);
    }
    void AttackPlayer()
    {
        attackCountdown = attackInterval;
        float rand = Random.Range(-10, 10);
        Vector3 offset = new(rand, rand, rand);
        Vector3 direction = (playerPos.position + offset) - transform.position;

        snakeAudioSource.spatialBlend = 0.2f;
        snakeAudioSource.pitch = Random.Range(1, 1.3f);
        snakeAudioSource.PlayOneShot(attackAudioClip);
        rbArray[0].AddForce(10*moveSpeed*direction);
        rbArray[8].AddForce(20 * moveSpeed * Vector3.down);

        rbArray[0].useGravity = true;
        
    }

    void SwitchToAlert()
    {
      
        transform.LookAt(playerPos);

        rbArray[0].useGravity = false;

        rbArray[0].AddForce(3*moveSpeed*Vector3.up);
        rbArray[6].AddForce(2*moveSpeed * Vector3.down);

        for(int i=7;i<rbArray.Length;i++)
        {
            rbArray[i].mass = 500;
        }
    }
    IEnumerator RecoverAfterAttack()
    {
        rbArray[0].AddForce(10 * moveSpeed * -transform.forward);
        yield return new WaitForSeconds(2);
        huntRadius = huntRadiusRange.x;
        alert = false;
        for (int i = 7; i < rbArray.Length; i++)
        {
            rbArray[i].mass = 100;
        }

        orientation = Random.Range(0, 2);

        if (orientation == 0)
        {
            StartCoroutine(TurnCoroutine(90));
        }

        if (orientation == 1)
        {
            StartCoroutine(TurnCoroutine(-90));
        }

    }
    IEnumerator TurnCoroutine(int turnAmount)
    {
        if (turnAmount > 0)
        {
            while (newRotation.y < turnAmount && !alert)
            {
                newRotation.y += .5f;
                newRotation.x = transform.localRotation.x;
                newRotation.z = transform.localRotation.z;
                transform.localRotation = Quaternion.Euler(newRotation);
                yield return null;
            }
        }
        else if (turnAmount < 0)
        {
            while (newRotation.y > turnAmount && !alert)
            {
                newRotation.y -= .5f;
                newRotation.x = transform.localRotation.x;
                newRotation.z = transform.localRotation.z;
                transform.localRotation = Quaternion.Euler(newRotation);
                yield return null;
            }
        }
    }
    void CheckBody()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform.name == "SnakeBody")
            {
                rbArray[0].AddForce(20*moveSpeed * Vector3.up);
                
            }
            else return;
        }
    }
    IEnumerator RespondToSonar()
    {
        float moveToPlayerCount = 4;
        while (moveToPlayerCount > 0)
        {
            moveToPlayerCount--;

            transform.LookAt(playerPos);
            Vector3 direction = playerPos.position - transform.position;
            transform.forward = direction;
            rbArray[0].AddForce(0.01f * moveSpeed * direction);
            yield return null;
        }

    }
    void CheckGravityMultiplier()
    {
        if (isGrounded || isUnderwater)
        {
            for(int i =0; i<rbArray.Length;i++)
            {
                rbArray[i].drag = 3;
            }
        }
        else
        {
            for (int i = 0; i < rbArray.Length; i++)
            {
                if (rbArray[i].drag > 0)
                {
                    rbArray[i].drag -= 0.05f;
                }
                else rbArray[i].drag = 0;
            }
           

        }

        if (!isGrounded && !isUnderwater)
        {
            airTime += Time.deltaTime;

            if (airTime > 1)
            {
                airTime = 0;
                for (int i = 0; i < rbArray.Length; i++)
                {
                    rbArray[i].useGravity = true;
                    rbArray[0].AddForce(0, -2*moveSpeed, 0.5f*moveSpeed);
                    transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
                }
            }
        }
    }

    IEnumerator SwitchToUnderwater()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < rbArray.Length; i++)
        {
            rbArray[i].useGravity = false;
        }
     
    }
    IEnumerator SlitherAnimation()
    {

        rbArray[1].AddForce(2*moveSpeed * slitherOffset * (transform.right));
        yield return new WaitForSeconds(2);
        rbArray[1].AddForce(2*moveSpeed * slitherOffset * (-transform.right));
      
    }

    void PlayIdleAudio()
    {
        idleAudioCountdown = idleAudioInterval;
        snakeAudioSource.spatialBlend = 0.5f;
        snakeAudioSource.pitch = Random.Range(0.7f, 1);
        snakeAudioSource.PlayOneShot(idleAudioClip[Random.Range(0, idleAudioClip.Length)]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sonar"))
        {
            StartCoroutine(RespondToSonar());
        }

        if(other.CompareTag("Lake"))
        {
            isUnderwater = true;
            StartCoroutine(SwitchToUnderwater());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lake"))
        {
            isUnderwater = false;
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

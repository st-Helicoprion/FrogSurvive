using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemyMovement : MonoBehaviour
{
    public Transform playerPos, snakeHead;
    public Rigidbody rb;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToCheck();
    }

    void MoveToCheck()
    {
        rb.AddForce(transform.forward * moveSpeed);
    }

    void CheckLeft()
    {

    }

    void CheckRight()
    {

    }

    IEnumerator RespondToSonar()
    {
        float moveToPlayerCount = 4;
        while (moveToPlayerCount > 0)
        {

            print("detected");
            moveToPlayerCount--;

            snakeHead.LookAt(playerPos);
            Vector3 direction = playerPos.position - transform.position;
            transform.forward = direction;
            rb.AddForce(0.01f * moveSpeed * direction);
            yield return null;
        }

    }

    IEnumerator NormalSlither()
    {
        yield return null;
    }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Sonar"))
            {
                StartCoroutine(RespondToSonar());
            }
        }
    }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemyMovement : MonoBehaviour
{
    public Transform playerPos;
    public Rigidbody snakeRB;
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
        snakeRB.AddForce(transform.forward * 10000);
    }

    void CheckLeft()
    {

    }

    void CheckRight()
    {

    }

    IEnumerator NormalSlither()
    {
        yield return null;
    }
}

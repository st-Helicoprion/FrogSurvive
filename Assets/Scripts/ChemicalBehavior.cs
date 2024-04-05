using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalBehavior : MonoBehaviour
{
    IEnumerator FlushDownAnimation()
    {
        while(transform.localPosition.y>-1)
        {
            transform.localPosition -= new Vector3(0, 0.01f, 0);
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(FlushDownAnimation());
        }
    }
}

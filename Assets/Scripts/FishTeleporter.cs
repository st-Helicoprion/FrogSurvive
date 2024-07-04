using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTeleporter : MonoBehaviour
{
    public Transform[] lakeMap;
    public float teleportCountdown, teleportInterval;
    public GameObject fish;
    public int teleportID;

    // Update is called once per frame
    void Update()
    {
        if(!AppUtilsManager.isPaused)
        {
            if (!FishEnemyMovement.alert)
            {
                
                    teleportCountdown -= Time.deltaTime;

                    if (teleportCountdown < 0)
                    {
                        if (FishEnemyMovement.targetLake == null)
                        {
                            teleportCountdown = teleportInterval;
                            RandomizeSpawnLake();
                            TeleportFish();
                            StartCoroutine(ActivateFishAfterTeleport());
                        }
                        else
                        {
                            for (int i = 0; i < lakeMap.Length; i++)
                            {
                                if (lakeMap[i].position == FishEnemyMovement.targetLake.GetChild(0).position)
                                {
                                    teleportCountdown = teleportInterval;
                                    teleportID = i;
                                    TeleportFish();
                                    StartCoroutine(ActivateFishAfterTeleport());
                                }
                            }
                        }

                    }
                
               
            }

            
        }
        
    }

    void RandomizeSpawnLake()
    {
        teleportID = Random.Range(0, lakeMap.Length);
    }
    void TeleportFish()
    {
        
        fish.SetActive(false);
       fish.transform.position = lakeMap[teleportID].position;
       
    }

    IEnumerator ActivateFishAfterTeleport()
    {
        yield return new WaitForSeconds(2);
        fish.SetActive(true);
        fish.transform.GetChild(0).transform.localPosition = Vector3.zero;
       
    }
}

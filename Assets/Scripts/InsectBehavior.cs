using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InsectBehavior : MonoBehaviour
{
    public enum InsectType { Worms, Firefly, Beetle, BabySpider};

    public InsectType insectType;

    public int behaviorColorID, behaviorNameID;

    public PlayerMovement playerMovement;
    public PlayerSonarManager playerSonar;
    public PlayerHealthManager playerHealth;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerSonar = FindObjectOfType<PlayerSonarManager>();
        playerHealth= FindObjectOfType<PlayerHealthManager>();
    }

    public void CheckInsectType()
    {
        if(insectType == InsectType.Firefly)
        {
            if(playerSonar.sonarIndex<2)
            {
                playerSonar.IncreaseSonarIndex();
            }
            behaviorColorID = 1;
            behaviorNameID = 1;
        }
        else if(insectType==InsectType.Worms)
        {
            behaviorColorID = 2;
            behaviorNameID = 2;
            PlayerStateManager.speedUp = true;
        }
        else if(insectType==InsectType.BabySpider)
        {
            playerHealth.playerHealth += 30;
            behaviorColorID = 3;
            behaviorNameID = 3;
        }
        else if(insectType==InsectType.Beetle)
        {
            behaviorColorID = 4;
            behaviorNameID = 4;
            PlayerStateManager.poisonUp = true;
        }
    }


}

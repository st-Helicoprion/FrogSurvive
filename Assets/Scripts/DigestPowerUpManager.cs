using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DigestPowerUpManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject digestUI;
    public Image powerUpRing;
    public int behaviorColorID, behaviorNameID;
    public PlayerSonarManager playerSonar;
    public PlayerHealthManager playerHealth;
    public PlayerStateManager playerState;
    public bool isDown;

    // Start is called before the first frame update
    void Start()
    {
        digestUI.SetActive(false);
        
        playerSonar = FindObjectOfType<PlayerSonarManager>();
        playerHealth = FindObjectOfType<PlayerHealthManager>();
        playerState = FindObjectOfType<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(powerUpRing.fillAmount == 0)
        {
            powerUpRing.fillAmount = 1;
            isDown= false;
            digestUI.SetActive(false);
            ActivatePowerUp();
        }

        if(isDown)
        {
            powerUpRing.fillAmount -= 0.02f;
        }
        else
        {
            powerUpRing.fillAmount += 0.02f;
        }
    }

    void SetRingColorAndShowUI()
    {
        powerUpRing.color = playerState.stateColor[behaviorColorID];
        digestUI.SetActive(true);
    }

    public void AnimateDigest()
    {
        isDown = true;
    }

    public void StopAnimateDigest()
    {
        isDown = false;
    }

    void ActivatePowerUp()
    {
        StartCoroutine(playerState.PowerUpPicked(behaviorNameID,behaviorColorID));

        if(behaviorColorID == 1)
        {
            if (playerSonar.sonarIndex < 2)
            {
                playerSonar.IncreaseSonarIndex();
            }
        }
        if (behaviorColorID == 2)
        {
            PlayerStateManager.speedUp = true;
        }
        if (behaviorColorID == 3)
        {
            playerHealth.playerHealth += 30;
        }
        if (behaviorColorID == 4)
        {
            PlayerStateManager.poisonUp = true;
        }
    }
    public void LoadPowerUpBuffer(InsectTriggerReporter insect)
    {
        behaviorNameID = insect.behavior.behaviorNameID;
        behaviorColorID= insect.behavior.behaviorColorID;

        SetRingColorAndShowUI();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AnimateDigest();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAnimateDigest();
    }
}
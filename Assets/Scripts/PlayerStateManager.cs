using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static bool isDead, isUnderwater, win;
    public static bool speedUp, poisonUp, bump;
    public GameObject waterParticle;
    public TextMeshPro stateText;
    public PlayerMovement playerMovement; public MeshRenderer[] mRenderer;
    public TrailRenderer trail; 
    public Material[] stateMaterials, playerMaterials; 
    public Color[] stateColor;
    public string[] stateName;
    public float powerUpLifetime, powerUpCountdown;
    public DigestPowerUpManager powerUpManager;
    public Transform bumper;
   
    private void Start()
    {
        ResetPlayerState();
        
    }
    // Update is called once per frame
    void Update()
    {
        CheckPlayerState();
    }

    void CheckPlayerState()
    {

        if(poisonUp||speedUp)
        {
            if(powerUpCountdown>0)
            {
                powerUpCountdown -= Time.deltaTime;
            }else ResetPlayerPowerUp();
        }

        if(speedUp)
        {
            playerMovement.speedMultiplier = 2;
            trail.startColor = stateColor[2];
           
        }

        if (poisonUp)
        {
            stateMaterials = new Material[] { playerMaterials[0], playerMaterials[2] };
            mRenderer[0].materials = stateMaterials;
            mRenderer[1].materials = stateMaterials;


        }
        else
        {
            stateMaterials = new Material[] { playerMaterials[0], playerMaterials[1] };
            mRenderer[0].materials = stateMaterials;
            mRenderer[1].materials = stateMaterials;
        }


        if (isDead)
        {
            PlayerDeath();
        }

        if(win)
        {
            TempEndGame();
            isDead = true;
        }

        if(bump)
        {
            bump = false;
            StartCoroutine(ActivateBumper());
        }

        if(PlayerSonarManager.hop)
        {
            PlayerSonarManager.hop = false;
            StartCoroutine(PlayerJump());
        }
        Color dimmerTrailColor = trail.startColor;
        dimmerTrailColor.a = 0.35f;
        trail.startColor = dimmerTrailColor;

    }

    IEnumerator PlayerJump()
    {
        stateText.enabled = true;
        stateText.text = stateName[6];
        stateText.color = stateColor[0];
        stateText.faceColor = stateColor[0];
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(StateTextAnimations(20, 40));
    }

    void PlayerDeath()
    {
        stateText.enabled = true;
        stateText.text = stateName[0];
        stateText.color = stateColor[0];
        stateText.faceColor = stateColor[0];
        playerMovement.enabled= false;

        StopAllCoroutines();
        Color textColor = stateText.color;
        textColor.a = 1;
        stateText.color = textColor;
    }

    void TempEndGame()
    {
        stateText.enabled = true;
        stateText.text = stateName[5];
        stateText.color = stateColor[0];
        stateText.faceColor = stateColor[0];
        playerMovement.enabled = false;
    }
    void ResetPlayerState()
    {
        isDead = false;
        win= false;
        stateText.enabled = false;
        playerMovement.enabled = true;
        ResetPlayerPowerUp();
    }

    void ResetPlayerPowerUp()
    {
        poisonUp = false;
        speedUp = false;
        powerUpCountdown = powerUpLifetime;
        playerMovement.speedMultiplier = 1;
        trail.startColor = stateColor[0];
        stateMaterials = new Material[] { playerMaterials[0], playerMaterials[1] };
        mRenderer[0].materials = stateMaterials;
        mRenderer[1].materials = stateMaterials;
    }

    public IEnumerator PowerUpPicked(int powTextID, int powColorID)
    {
        powerUpCountdown= powerUpLifetime;
        stateText.enabled= true;
        stateText.text = stateName[powTextID];
        stateText.color = stateColor[powColorID];
        stateText.faceColor = stateColor[powColorID];
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(StateTextAnimations(50, 100));
        
        
    }

    IEnumerator StateTextAnimations(int charDist, int splitSpeed)
    {
        Color animTextColor = stateText.color;
        while (stateText.characterSpacing < charDist)
        {
            stateText.characterSpacing += splitSpeed * Time.deltaTime;
            animTextColor.a -= 1.75f * Time.deltaTime;
            stateText.color = animTextColor;
            yield return null;
        }
        stateText.enabled = false;
        stateText.characterSpacing = 0;
        animTextColor.a = 1;
        stateText.color= animTextColor;
       
    }

    IEnumerator ActivateBumper()
    {
        bumper.localScale = new Vector3(8, 8, 8);
        yield return new WaitForSeconds(0.5f);
        while(bumper.localScale.x > 0)
        {
            bumper.localScale = Vector3.Lerp(bumper.localScale, Vector3.zero, 2 * Time.deltaTime);
            yield return null;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PuddleBehavior puddle))
        {
            puddle.DisablePuddle();

            Instantiate(waterParticle, transform.position, Quaternion.identity);
            
        }

        if(other.TryGetComponent(out InsectTriggerReporter insect))
        {
            insect.SaveBehaviorToBuffer();
            powerUpManager.LoadPowerUpBuffer(insect);
            insect.DisableInsect();
        }

        if(other.CompareTag("Lake"))
        {
            isUnderwater= true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Lake"))
        {
            isUnderwater= false;
            
        }
    }
}

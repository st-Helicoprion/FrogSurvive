using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager instance;

    public RectTransform healthbar;
    public float playerHealth;
    public VolumeProfile volumeProfile;

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       if(volumeProfile.TryGet<Vignette>(out Vignette vignette))
        {
            vignette.intensity.value = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LimitPlayerHealth();
        if(!PlayerStateManager.isDead)
        {
            if(!PlayerStateManager.isUnderwater&& !RainManager.isRaining)
            {
                PlayerHealthDecay();
            }
            else
            {
                FullHealPlayer();
            }

        }

    }

    void LimitPlayerHealth()
    {
        playerHealth = Mathf.Clamp(playerHealth, 0, 150);
        healthbar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, playerHealth);
    }

    void PlayerHealthDecay()
    {
        if (playerHealth > 0)
        {
            playerHealth -= Time.deltaTime;
        }
        else
        {
            PlayerStateManager.isDead = true;
        }

        
    }

    void FullHealPlayer()
    {
        playerHealth += 15*Time.deltaTime;
    }
}

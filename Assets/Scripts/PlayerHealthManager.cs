using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager instance;

    public RectTransform healthbar;
    public float playerHealth;

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
       
    }

    // Update is called once per frame
    void Update()
    {
        LimitPlayerHealth();
        if(!PlayerStateManager.isUnderwater)
        {
         PlayerHealthDecay();

        }
        else
        {
            FullHealPlayer();
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

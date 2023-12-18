using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static bool isDead;
    public TextMeshProUGUI stateText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerState();
    }

    void CheckPlayerState()
    {
        if (isDead)
        {
            PlayerDeath();
        }
        else return;
    }

    void HealPlayer(float healAmount)
    {
        PlayerHealthManager.instance.playerHealth += healAmount;
    }

    void PlayerDeath()
    {
        stateText.enabled = true;
        stateText.text = "slain";

        print("player is dead");

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Puddle"))
        {
            HealPlayer(other.GetComponent<PuddleBehavior>().healAmount);
            Destroy(other.gameObject);
        }
    }
}

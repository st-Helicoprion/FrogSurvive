using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BossWeaponHitReporter : MonoBehaviour
{
    public enum BossType { Snake, Owl, Fish, Spider };
    public BossType bossType;
    public PlayerHealthManager playerHealth;
    public float damage;
    public VolumeProfile volumeProfile;

    void GroundCollisionResponseByType()
    {
        if (bossType == BossType.Owl)
        {
            OwlEnemyMovement.recoverAfterAttack = true;
            OwlEnemyMovement.hitAltitude = transform.position;
        }
    }

    void DamagePlayerHealth()
    {
        print("hit");
        playerHealth.playerHealth -= damage;

        
        if(volumeProfile.TryGet<Vignette>(out Vignette vignette))
        {
            vignette.intensity.value = 0.6f;
            StartCoroutine(AnimateHit(vignette));
        }
    }

    IEnumerator AnimateHit(Vignette vignette)
    {
        while(vignette.intensity.value>0)
        {
            vignette.intensity.value -= 0.002f;
            yield return null;
        }
        
    }

    void RemovePlayerShield()
    {
        print("armor removed");
        PlayerStateManager.poisonUp = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Lake"))
        {
            GroundCollisionResponseByType();
        }

        if (other.CompareTag("Player"))
        {
            if (!PlayerStateManager.poisonUp)
            {
                DamagePlayerHealth();

            }
            else
            {
                RemovePlayerShield();
                GroundCollisionResponseByType();
            }

        }
    }
}
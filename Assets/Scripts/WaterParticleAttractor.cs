using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticleAttractor : MonoBehaviour
{
    public ParticleSystem pSystem;
    public ParticleSystem.Particle[] particles = new ParticleSystem.Particle[6];
    public Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
       
        pSystem.collision.AddPlane(GameObject.Find("Terrain").transform);
        pSystem.trigger.AddCollider(GameObject.Find("Player").transform);

        playerPos = GameObject.Find("Player").transform;

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (pSystem.isPlaying)
        {
            int length = pSystem.GetParticles(particles);
            for (int i = 0; i < length; i++)
            {
                particles[i].position += 8*Time.deltaTime*((playerPos.position - particles[i].position) / (particles[i].remainingLifetime));
            }
                pSystem.SetParticles(particles, length);
        }
            
        
      
            
        
    }
}

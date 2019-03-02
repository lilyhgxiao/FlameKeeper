using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowParticles : MonoBehaviour
{
    public ParticleSystem particles;
    public PlayerControllerSimple player;
    public float scaleCoefficient = 0.5f;

    private List<ParticleCollisionEvent> particleCollisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particleCollisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called when a particle of this particle system collides with 
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            /* For every particle the player collides with, temporarely scale down their flame.
             * If the flame is scled below 0, respawn the player at their last checkpoint.
             */
            int collisionCount = ParticlePhysicsExtensions.GetCollisionEvents(particles, other, particleCollisionEvents);
            float scaleLightToo = 1.0f - scaleCoefficient * collisionCount;
            player.ScaleLightSource(scaleLightToo);
            if (scaleLightToo <= 0)
            {
                player.GoToLastCheckpoint();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringWater : MonoBehaviour
{
    public ParticleSystem particles;
    public float duration = 0.0f;
    public float pauseTime = 0.0f;
    public PlayerControllerSimple player;

    private float timer = 0.0f;
    private List<ParticleCollisionEvent> particleCollisionEvents;


    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particleCollisionEvents = new List<ParticleCollisionEvent>();
        timer = -particles.main.startDelay.constant;
        particles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Rune for 'duration', pause for 'duration + pauseTime', then repeat. 
         */
        if (timer > duration)
        {
            particles.Stop();
        }
        if (timer > duration + pauseTime)
        {
            timer = -particles.main.startDelay.constant;
            particles.Play();
        }
        timer += Time.deltaTime;
        //Debug.Log(timer);
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
            float scaleLightToo = 1.0f - 0.02f * collisionCount;
            player.ScaleLightSource(scaleLightToo);
            if (scaleLightToo <= 0)
            {
                player.GoToLastCheckpoint();
            }
        }
    }
}

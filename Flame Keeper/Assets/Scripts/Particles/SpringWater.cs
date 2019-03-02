using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringWater : MonoBehaviour
{
    public ParticleSystem particles;
    public float duration = 0.0f;
    public float pauseTime = 0.0f;
    public PlayerControllerSimple player;

    public bool enableRandomDuration = false;
    public float minDuration = 0.0f, maxDuration = 0.0f;

    public bool enableRandomPauseTime = false;
    public float minPauseTime = 0.0f, maxPauseTime = 0.0f;

    public float scaleCoefficient = 0.02f;

    private float timer = 0.0f;
    private List<ParticleCollisionEvent> particleCollisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particleCollisionEvents = new List<ParticleCollisionEvent>();
        // Account for startdelay for timer. timer == 0 implies simulation starts now. 
        timer = -particles.main.startDelay.constant;
        particles.Play();

        if (enableRandomDuration)
        {
            duration = Random.Range(minDuration, maxDuration);
            Debug.Log("Starting Random Duration = " + duration);
        }

        if (enableRandomPauseTime)
        {
            pauseTime = Random.Range(minPauseTime, maxPauseTime);
            Debug.Log("Starting Random Pause Time = " + pauseTime);
        }
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
            if (enableRandomDuration)
            {
                duration = Random.Range(minDuration, maxDuration);
                Debug.Log("New Random Duration = " + duration);
            }

            if (enableRandomPauseTime)
            {
                pauseTime = Random.Range(minPauseTime, maxPauseTime);
                Debug.Log("New Random Pause Time = " + pauseTime);
            }

            // Want to account for extra delay time.
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
            float scaleLightToo = 1.0f - scaleCoefficient * collisionCount;
            player.ScaleLightSource(scaleLightToo);
            if (scaleLightToo <= 0)
            {
                player.GoToLastCheckpoint();
            }
        }
    }
}

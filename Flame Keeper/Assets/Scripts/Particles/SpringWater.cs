using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringWater : MonoBehaviour
{
    // Reference to the particle system this script is attached too.
    public ParticleSystem particles;
    // How long the simulation lasts before looping.
    public float duration = 0.0f;
    // How long to pause before starting next loop.
    public float pauseTime = 0.0f;
    // Reference to player.
    public PlayerControllerSimple player;

    // When true, assigns a random value to duration.
    public bool enableRandomDuration = false;
    // Random value bounds for duration.
    public float minDuration = 0.0f, maxDuration = 0.0f;

    // When true, assigns a random value to pauseTime.
    public bool enableRandomPauseTime = false;
    // Random value bounds for pauseTime.
    public float minPauseTime = 0.0f, maxPauseTime = 0.0f;

    public float scaleCoefficient = 0.02f;

    // Used to keep track of how long a single loop of the duration is running.
    private float timer = 0.0f;
    // Stores event information for each collided particle per collision.
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

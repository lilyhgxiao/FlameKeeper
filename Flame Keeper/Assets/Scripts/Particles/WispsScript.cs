using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispsScript : MonoBehaviour
{
    public ParticleSystem particles;
    public PlayerControllerSimple player;
    public float scaleCoefficient = 0.4f;
    public float emitionTime;

    private List<ParticleCollisionEvent> particleCollisionEvents;
    private int collisionCount;
    private ParticleSystem.Particle[] currentParticles;
    private int currentParticleCount;


    // Start is called before the first frame update
    void Start()
    {
        particleCollisionEvents = new List<ParticleCollisionEvent>();
        particles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentParticles == null || currentParticles.Length < particles.main.maxParticles)
        {
            currentParticles = new ParticleSystem.Particle[particles.main.maxParticles];
        }

        currentParticleCount = particles.GetParticles(currentParticles);
        for (int i = 0; i < currentParticleCount; i++)
        {
            // currentParticles[i].remainingLifetime = currentParticles[i].startLifetime;
            currentParticles[i].remainingLifetime = currentParticles[i].startLifetime - 1;
        }
        particles.SetParticles(currentParticles);
        emitionTime -= Time.deltaTime;
        if (emitionTime < 0)
        {
            particles.Stop();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided with Player");
            int collisionCount = ParticlePhysicsExtensions.GetCollisionEvents(particles, other, particleCollisionEvents);
            float scaleLightToo = 1.0f - scaleCoefficient * collisionCount;
            Debug.Log("Scale Light Too = " + scaleLightToo);
            player.ScaleLightSource(scaleLightToo);
            if (scaleLightToo <= 0)
            {
                player.GoToLastCheckpoint();
            }
        }
    }
}

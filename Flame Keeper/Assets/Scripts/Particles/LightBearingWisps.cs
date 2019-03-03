using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBearingWisps : MonoBehaviour
{
    public ParticleSystem particles;
    public PlayerControllerSimple player;
    public float scaleCoefficient = -0.4f;

    private List<ParticleCollisionEvent> particleCollisionEvents;
    private int collisionCount;

    // Start is called before the first frame update
    void Start()
    {
        particleCollisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {

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

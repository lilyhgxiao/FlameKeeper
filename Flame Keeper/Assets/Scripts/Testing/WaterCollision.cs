using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    
    public float waitTime = 3f;
    public GameObject player;
    private bool playerTouching;
    private bool playerAlive;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        playerTouching = false;
        playerAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTouching && playerAlive)
        {
            timer += Time.deltaTime;
            player.GetComponent<Renderer>().material.color = Color.blue;
            if (timer > waitTime)
            {
                player.GetComponent<Renderer>().material.color = Color.red;
                playerAlive = false;
                timer = 0f;
            }
        }
        else
        {
            timer = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        playerTouching = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        playerTouching = false;
        playerAlive = true;
        player.GetComponent<Renderer>().material.color = Color.white;
    }
}
